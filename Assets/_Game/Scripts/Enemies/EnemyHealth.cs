using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    
    public float maxHealth = 100f;
    public float health = 100f;
    public Animator animator;
    private static readonly int Dead = Animator.StringToHash("Dead");

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
            animator.SetTrigger(Dead);
            Destroy(gameObject, 1.0f);
        }
    }

    public void Heal(float amount)
    {
        print("healing Enemy");
        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }
}
