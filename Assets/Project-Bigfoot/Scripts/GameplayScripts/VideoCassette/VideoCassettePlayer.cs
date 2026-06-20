using UnityEngine;
using MoreMountains.Feedbacks;

[System.Serializable]
public class CassetteFeedbackEntry
{
    public int cassetteID;
    public MMF_Player feedback;
}

public class VideoCassettePlayer : InteractableObject
{
    [Header("Cassette Audio Database")]
    [SerializeField] private CassetteFeedbackEntry[] cassetteFeedbacks;

    public override void Interact()
    {
        InteractableObject[] items = PlayerInventory.Instance.GetItemsForUI();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            int id = items[i].GetID();

            for (int j = 0; j < cassetteFeedbacks.Length; j++)
            {
                if (cassetteFeedbacks[j].cassetteID == id)
                {
                    cassetteFeedbacks[j].feedback.PlayFeedbacks();

                    PlayerInventory.Instance.RemoveObject(items[i]);

                    Debug.Log("Cassette played: " + id);

                    return;
                }
            }
        }

        Debug.Log("No cassette found");
    }
}
