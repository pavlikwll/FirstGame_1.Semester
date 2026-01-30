using UnityEngine;

public class ShadowPatrolMovement : MonoBehaviour
{
    public enum FacingDirection { Left, Right }
    public FacingDirection facingDirection = FacingDirection.Right;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ShadowEnvironmentChecker environmentChecker;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Turn Settings")]
    [SerializeField] private float turnCooldown = 0.15f;

    private float turnTimer;
    
    private void FlipIfNeeded()
    {
        if (facingDirection == FacingDirection.Right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Awake()
    {
        FlipIfNeeded();
    }

    private void Patrol()
    {
        if (turnTimer <= 0f && environmentChecker && environmentChecker.NeedTurn())
        {
            ChangeDirection();
            
            // короткий кулдаун, щоб не смикало на краю
            turnTimer = turnCooldown;
        }

        rb.linearVelocity = new Vector2(patrolSpeed * DirectionSign(), rb.linearVelocity.y);
    }
    
    private void FixedUpdate()
    {
        if (turnTimer > 0f)
        {
            turnTimer -= Time.deltaTime;
        }

        Patrol();
    }

    

    public void ChangeDirection()
    {
        if (facingDirection == FacingDirection.Right)
        {
            facingDirection = FacingDirection.Left;
        }
        else
        {
            facingDirection = FacingDirection.Right;
        }

        FlipIfNeeded();
    }
    
    public int DirectionSign()
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
}
