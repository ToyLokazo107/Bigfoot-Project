using UnityEngine;

public class JumpScareTrigger : MonoBehaviour
{
    [SerializeField] private JumpScareUI jumpScareUI;
    [SerializeField] private Sprite jumpScareSprite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jumpScareUI.Trigger();

        }
    }
}