using UnityEngine;
using MoreMountains.Feedbacks;

public class JumpScareUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private MMF_Player jumpScareSound;

    [SerializeField] private float fadeOutSpeed = 2f;

    public void Trigger()
    {
        canvasGroup.alpha = 1;

        jumpScareSound.PlayFeedbacks();
    }

    private void Update()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeOutSpeed;
        }
    }
}