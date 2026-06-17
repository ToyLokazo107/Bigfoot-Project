using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    [SerializeField] private GameObject invisibleWall;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (BookManager.Instance.GetNotesArchived() < 5)
        {
            Debug.Log("I need more information before entering.");
            return;
        }

        Debug.Log("I can enter the cave now.");
        invisibleWall.SetActive(false);
    }
}
