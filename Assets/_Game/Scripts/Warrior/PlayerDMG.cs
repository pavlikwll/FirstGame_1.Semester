using UnityEngine;

public class PlayerDMG : MonoBehaviour
{
    public float damage = 30f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BAM");

        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            Debug.Log("enemy found");
            enemyHealth.TakeDamage(damage);
        }
        else
        {
            Debug.Log("no enemyHealth");
        }
        
        EnvironmentHealth environmentHealth = other.GetComponentInParent<EnvironmentHealth>();
        
        if (environmentHealth != null)
        {
            Debug.Log("enemy found");
            environmentHealth.TakeDamage(damage);
        }
        else
        {
            Debug.Log("no enemyHealth");
        }
    }
}