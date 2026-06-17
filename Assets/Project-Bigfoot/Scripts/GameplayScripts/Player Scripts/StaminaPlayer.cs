using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    public float currentStamina = 100f;
    public float maxStamina = 100f;
    public float staminaRate = 10f;

    public bool isRecharging = false;

    public void DecreaseStamina()
    {
        if (currentStamina > 0 && !isRecharging)
        {
            currentStamina -= Time.deltaTime * staminaRate;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isRecharging = true;
            }
        }
    }

    public void IncreaseStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * staminaRate;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isRecharging = false;
            }
        }
    }

    public bool CanSprint()
    {
        return currentStamina > 0 && !isRecharging;
    }
}
