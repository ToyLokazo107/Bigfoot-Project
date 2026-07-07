using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BigfootHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth = 10;

    public string victorySceneName = "Victory";

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

        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(victorySceneName);
    }
}