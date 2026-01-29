using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    
    public float maxHealth = 100f;
    public float health = 100f;
    public Health warriorHealth;
    public Animator animator;
    private static readonly int Dead = Animator.StringToHash("Dead");

    [Header("UI")]
    
    public Image healthBar;

    void Start()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            print("updating healthbar");
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0f, 1f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHealthBar();

        if (health <= 0)
        {
            Debug.Log("Player Died");
            animator.SetTrigger(Dead);
            Destroy(gameObject, 1.0f);
        }
    }

    public void Heal(float amount)
    {
        print("healing");
        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHealthBar();
    }
}