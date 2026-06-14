using TMPro;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager instance;

    public TMP_Text countertext;

    public int notesArchived = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AgregarNota()
    {
        notesArchived++;

        countertext.text =  "Notes: " + notesArchived + "/5";
    }
}