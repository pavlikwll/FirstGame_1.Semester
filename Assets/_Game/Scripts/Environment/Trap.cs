using UnityEngine;

public class TRap : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Destroy(gameObject, 1.0f);
            }
        }
        
        EnvironmentHealth environmentHealth = other.GetComponent<EnvironmentHealth>();
        if (environmentHealth != null)
        {
            environmentHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}