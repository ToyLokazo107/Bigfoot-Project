using UnityEngine;
using UnityEngine.UI;

public class BigfootHealthUI : MonoBehaviour
{
    public Image fillImage;
    public BigfootHealth bigfootHealth;

    private void Update()
    {
        fillImage.fillAmount = (float)bigfootHealth.currentHealth / bigfootHealth.maxHealth;
    }
}