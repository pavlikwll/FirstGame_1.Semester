using UnityEngine;

public class PlayerDMG : MonoBehaviour
{
    [SerializeField] public float damage = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BAM");

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            Debug.Log("enemy found");
            enemyHealth.TakeDamage(damage);
            return;
        }
        else
        {
            Debug.Log("no enemyHealth");
        }
        
        EnvironmentHealth environmentHealth = other.GetComponentInParent<EnvironmentHealth>();
        
        if (environmentHealth != null)
        {
            Debug.Log("something found");
            environmentHealth.TakeDamage(damage);
            return;
        }
        else
        {
            Debug.Log("no more something");
        }
    }
}