using UnityEngine;
using MoreMountains.Feedbacks;

public class AudioGameplay : MonoBehaviour
{
    public MMF_Player feelAudio;

    void Start()
    {
        feelAudio.PlayFeedbacks();
    }
}