using UnityEngine;

public class AshisChecking : MonoBehaviour
{
    [Header("Chick-Chirick")]
    public Transform Warrior;
    public Transform Ash;
    private bool WarriorIn;

    private void Update()
    {
        // повертаємося в бік гравця
        if (Warrior.position.x > transform.position.x)
        {
            Ash.transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("turning");
        }
        else
        {
            Ash.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        WarriorIn = true;
        Debug.Log("player in");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        WarriorIn = false;
        Debug.Log("player out");
    }
}