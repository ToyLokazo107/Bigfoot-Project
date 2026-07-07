using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image fillImage;
    public PlayerHealth playerHealth;

    private void Update()
    {
        fillImage.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
