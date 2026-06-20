using TMPro;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    [SerializeField] private TMP_Text counterText;

    private int notesArchived = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCounter();
    }

    public void AddNote()
    {
        notesArchived++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        counterText.text = "Notes: " + notesArchived + "/4";
    }
    public int GetNotesArchived()
    {
        return notesArchived;
    }
}