using UnityEngine;
using UnityEngine.InputSystem;

namespace ___WorkData.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region ENUMS
        public enum PlayerActionState { Default, Attack, Jump, Roll }
        public enum PlayerMovementState { Idle, Run }
        public enum FacingDirection { Left, Right }
        #endregion
        
        #region ANIMATOR HASHES
        // Хеші параметрів аніматора (швидше й без помилок у рядках)
        public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
        public static readonly int Hash_ActionID = Animator.StringToHash("ActionID");
        public static readonly int Hash_ActionTrigger = Animator.StringToHash("ActionTrigger");
        public static readonly int Hash_Grounded = Animator.StringToHash("Grounded");
        public static readonly int Hash_ClickCount = Animator.StringToHash("clickCount");
        #endregion

        #region INSPECTOR VARIABLES
        [Header("Movement")]
        [SerializeField] private float walkingSpeed = 8f;
        [SerializeField] private float jumpForce = 8f;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector2 groundBoxSize; // Розмір боксу перевірки землі
        [SerializeField] private Vector2 groundBoxPosition; // Зсув боксу відносно персонажа
        
        [Header("Combo System")]
        [SerializeField] private float doubleClickTime = 0.25f;
        private int clickCount = 0;
        private float clickTimer;
        
        [Header("Roll Settings")]
        [SerializeField] private float rollForce = 20f;
        [SerializeField] private float rollDuration = 0.25f;
        private bool Rolling;
        private bool canRoll = true;
        private float rollTimer;
        
        private PlayerPlatformHandler playerPlatformHandler;
        #endregion

        #region CACHED VARIABLES
        private Animator animator;
        private Rigidbody2D rb;
        private InputSystem_Actions inputActions;
        private PlayerInteractions playerInteractions;

        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction attackAction;
        private InputAction rollAction;
        private InputAction interactAction;

        private Vector2 moveInput;
        public bool grounded;

        //private bool Jumping;
        private bool canJump = true;

        public PlayerMovementState movementState;
        public PlayerActionState actionState;
        public FacingDirection facingDirection;
        #endregion

        #region UNITY LIFECYCLE
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            inputActions = new InputSystem_Actions();

            moveAction = inputActions.Player.Move;
            jumpAction = inputActions.Player.Jump;
            attackAction = inputActions.Player.Attack;
            rollAction = inputActions.Player.Roll;

            facingDirection = FacingDirection.Right;
            actionState = PlayerActionState.Default;

            playerPlatformHandler = GetComponent<PlayerPlatformHandler>();
            
            playerInteractions = GetComponent<PlayerInteractions>();
            interactAction = inputActions.Player.Interact;
        }

        private void OnEnable()
        {
            inputActions.Enable();
            moveAction.performed += Move;
            moveAction.canceled += Move;
            jumpAction.performed += Jump;
            attackAction.performed += Attack;
            rollAction.performed += Roll;
            interactAction.performed += Interact;
        }

        private void OnDisable()
        {
            inputActions.Disable();
            moveAction.performed -= Move;
            moveAction.canceled -= Move;
            jumpAction.performed -= Jump;
            attackAction.performed -= Attack;
            rollAction.performed -= Roll;
            interactAction.performed -= Interact;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleComboTimer();
            Grounded();
            HandleRoll();
            // Основна логіка руху в фізичному апдейті
            UpdateAnimator();
        }
        #endregion

        #region PHYSICS
        public void Grounded()
        {
            grounded = Physics2D.OverlapBox((Vector2)transform.position + groundBoxPosition, groundBoxSize, 0f, groundLayer);
            animator.SetBool(Hash_Grounded, grounded);

            // Якщо приземлилися після стрибка - повертаємося в Default
            if (grounded && actionState == PlayerActionState.Jump)
            {
                actionState = PlayerActionState.Default;
            }
        }
        #endregion

        #region MOVEMENT
        private void HandleMovement()
        {
            // Під час рола рух від інпуту блокується
            if (Rolling) return;

            // moveInput.x дає -1..1, множу на швидкість
            rb.linearVelocity = new Vector2(moveInput.x * walkingSpeed, rb.linearVelocity.y);

            if (Mathf.Abs(moveInput.x) > 0.01f)
            {
                movementState = PlayerMovementState.Run;
                FlipIfNeeded();
            }
            else
            {
                movementState = PlayerMovementState.Idle;
            }
        }

        private void FlipIfNeeded()
        {
            if (moveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                facingDirection = FacingDirection.Right;
            }
            else if (moveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingDirection = FacingDirection.Left;
            }
        }
        
        private int DirectionSign()
            // Повертає +1 або -1 залежно від напряму
        {
            if (facingDirection == FacingDirection.Right)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region JUMP
        private void Jump(InputAction.CallbackContext ctx)
        {
            if (!grounded || !canJump) return;

            //Jumping = true;
            canJump = false;
            actionState = PlayerActionState.Jump;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            AnimationSetActionID(1);
        }

        private void AnimEvent_EndJump()
        {
            //Jumping = false;
            canJump = true;
            actionState = PlayerActionState.Default;
        }
        #endregion
        
        private void Interact(InputAction.CallbackContext ctx)
            // Взаємодія з об’єктами
        {
            if(playerInteractions == null)
            {
                Debug.Log("No PlayerInteraction component to the player attached");
                return;
            }
        
            playerInteractions.TryInteract();
        }

        #region ATTACK / COMBO
        private void Attack(InputAction.CallbackContext ctx)
        {
            if (!grounded)
            {
                return;
            }
            // Якщо вже в атаці і кліків більше ніж 1 - не даю спамити
            if (actionState == PlayerActionState.Attack && clickCount > 1)
            {
                print("Spam protected");
                return;
            }
            print("spamming");
            clickCount++;
            animator.SetInteger(Hash_ClickCount, clickCount);

            actionState = PlayerActionState.Attack;

            if (clickCount == 1)
            {
                AnimationSetActionID(9);
                
                clickTimer = doubleClickTime;
            }
            
            /*
            else if (clickCount == 2)
            {
                AnimationSetActionID(10);
 
                clickTimer = 0;
            }
            */
        }

        private void HandleComboTimer()
            // Таймер між кліками для комбо
        {
            if (clickTimer > 0)
            {
                clickTimer -= Time.deltaTime;
               // if (clickTimer <= 0) clickCount = 0;
            }
        }

        public void EndAttack()
        {
            print("EndATtack");
            actionState = PlayerActionState.Default;
            
            clickCount = 0;
            animator.SetInteger(Hash_ClickCount, clickCount);

        }
        #endregion

        #region ROLL
        private void Roll(InputAction.CallbackContext ctx)
        {
            if (!grounded || !canRoll || Rolling)
            {
                return;
            }

            canRoll = false;
            Rolling = true;
            rollTimer = rollDuration;

            actionState = PlayerActionState.Roll;

            // Ривок у бік, куди дивиться персонаж
            rb.linearVelocity = new Vector2(DirectionSign() * rollForce, 0);

            AnimationSetActionID(2);
        }

        private void HandleRoll()
        {
            if (Rolling)
            {
                rollTimer -= Time.deltaTime;
                if (rollTimer <= 0f)
                {
                    Rolling = false;
                }
            }
        }

        private void AnimEvent_EndRoll()
        {
            Rolling = false;
            canRoll = true;
            actionState = PlayerActionState.Default;
        }
        #endregion

        #region INPUT CALLBACK
        private void Move(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();

            // Якщо тиснемо вниз - проходимо крізь one-way платформу
            if (moveInput.y < -0.5f)
            {
                playerPlatformHandler.TryDisableOneWayEffector();
            }
        }
        #endregion

        #region ANIMATOR UPDATE
        private void UpdateAnimator()
        {
            // Передаємо швидкість у аніматор
            animator.SetFloat(Hash_MovementValue, Mathf.Abs(rb.linearVelocity.x));
        }

        private void AnimationSetActionID(int id)
        {
            animator.SetTrigger(Hash_ActionTrigger);
            animator.SetInteger(Hash_ActionID, id);
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((Vector2)transform.position + groundBoxPosition, groundBoxSize);
        }
    }
}