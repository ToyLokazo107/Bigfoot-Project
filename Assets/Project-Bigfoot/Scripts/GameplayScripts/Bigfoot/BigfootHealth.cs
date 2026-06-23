using UnityEngine;

public class BigfootHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Bigfoot HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("BIGFOOT DEFEATED");
        gameObject.SetActive(false);
    }
}