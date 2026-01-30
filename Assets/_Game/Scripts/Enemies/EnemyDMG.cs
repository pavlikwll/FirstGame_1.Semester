using UnityEngine;

public class EnemyDMG : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    [Header("Damage")]
    [SerializeField] private float damage = 5f;
    //[SerializeField] private float cooldown = 1f;
    
    [Header("References")]
    [SerializeField] private Animator animator;

    //private float timer;
    //private bool WarriorIn;
    
    /*
    private void FixedUpdate()
    {
        // відлік кулдауну
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        // якщо гравець у зоні і кулдаун минув - б’ємо
        if (WarriorIn && timer <= 0f)
        {
            timer = cooldown;
        }
    }
    */
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        //arriorIn = true;
        Debug.Log("inside");
        // вмикаємо анімацію атаки
        if (animator)
        {
            animator.SetBool(Attack, true);
        }

        /*
        // можна вдарити одразу при вході
        if (timer <= 0f)
        {
            timer = cooldown;
        }
        */
        
        Health warriorHealth = other.GetComponent<Health>();
        if (warriorHealth != null)
        {
            warriorHealth.TakeDamage(damage);
        }
    }

/*
private void OnTriggerStay2D(Collider2D other)
{
   if (!other.CompareTag("Player"))
   {
       return;
   }

   Health warriorHealth = other.GetComponent<Health>();
   if (warriorHealth != null)
   {
       warriorHealth.TakeDamage(damage * Time.deltaTime);
   }
}
*/

private void OnTriggerExit2D(Collider2D other)
{

if (!other.CompareTag("Player"))
{
return;
}

//WarriorIn = false;

// вимикаємо анімацію атаки
if (animator)
{
   animator.SetBool(Attack, false);
}
}
}
