using UnityEngine;
using MoreMountains.Feedbacks;

public class AudioGameplay : MonoBehaviour
{
    public MMF_Player feelAudio;

    void Start()
    {
        if (feelAudio != null)
        {
            feelAudio.PlayFeedbacks();
        }
    }

    private void OnDestroy()
    {
        if (feelAudio != null)
        {
            feelAudio.StopFeedbacks();
        }
    }
}