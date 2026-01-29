using UnityEngine;

public class EnvironmentHealth : MonoBehaviour
{
    [Header("Health Settings")]
    
    public float maxHealth = 5f;
    public float health = 5f;

    void Start()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);

        if (health <= 0)
        {
            Debug.Log("Enemy Died");
            Destroy(gameObject);
        }
    }
}
