using System;
using UnityEngine;

public class Regeneration : MonoBehaviour
{
    [Header("Settings")] public float regenerationPerSecond = 10f;

    public Health playerHealth;
    public EnemyHealth enemyHealth;

    public bool somethingIn = false;


    private void Update()
    {
        if (!somethingIn) return;

        playerHealth.Heal(regenerationPerSecond * Time.deltaTime);
    }

    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                print("PLAYER found");
                health.Heal(regenerationPerSecond * Time.deltaTime);
            }
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("something entered");
        if (other.CompareTag("Player"))
        {
            somethingIn = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            somethingIn = false;
        }
    }
}