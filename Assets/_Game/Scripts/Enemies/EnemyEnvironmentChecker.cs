using UnityEngine;

public class ShadowEnvironmentChecker : MonoBehaviour
{
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool grounded = true;
    
    [SerializeField] private float wallDistance = 0.2f;
    
    [SerializeField] private LayerMask groundAndWallMask;
    [SerializeField] private Vector2 groundBoxSize;
    [SerializeField] private Vector2 groundBoxPosition;

    private ShadowPatrolMovement shadowPatrolMovement;

    private void Awake()
    {
        shadowPatrolMovement = GetComponent<ShadowPatrolMovement>();
    }

    public bool NeedTurn()
    {
        if (!shadowPatrolMovement || !wallCheck)
        {
            return false;
        }

        return WallAhead() || NoGround();
        
/*
         if (!shadowPatrolMovement && !wallCheck || !groundCheck)
         {
             return false;
         }
*/
        
        // якщо попереду стіна або закінчилася земля — розвертаємося
    }

    private bool WallAhead()
    {
        Vector2 direction = Vector2.right * shadowPatrolMovement.DirectionSign();
        
        Debug.DrawRay(wallCheck.position, direction * wallDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, direction, wallDistance, groundAndWallMask);
        
        // якщо попереду щось є - повертаємо true
        return hit.collider != null;
    }

    private bool NoGround()
    {
        grounded = Physics2D.OverlapBox((Vector2)transform.position + groundBoxPosition, groundBoxSize, 0f, groundAndWallMask);
        return !grounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundBoxPosition, groundBoxSize);
    }
}
