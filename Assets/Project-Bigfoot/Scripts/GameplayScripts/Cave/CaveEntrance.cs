using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    public GameObject paredInvisible;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (BookManager.instance.notesArchived < 5)
        {
            Debug.Log("Necesito encontrar más información.");
            return;
        }

        Debug.Log("Puedo entrar a la cueva.");

        paredInvisible.SetActive(false);
    }
}
