using UnityEngine;
using UnityEngine.UI;

public class BarraStamina : MonoBehaviour
{
    public Image fillImage;

    [SerializeField]private StaminaPlayer player;
    private float maxStamina;

    private void Start()
    {
        fillImage = GetComponent<Image>();

        //player = GameObject.FindWithTag("Player").GetComponent<StaminaPlayer>();

        maxStamina = player.maxStamina;
    }

    private void Update()
    {
        fillImage.fillAmount = player.currentStamina / player.maxStamina;
    }
}
