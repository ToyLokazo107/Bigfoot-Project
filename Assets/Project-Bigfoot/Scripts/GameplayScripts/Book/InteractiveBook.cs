using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveBook : InteractableObject
{
    [Header("Note Text Entries")]
    [SerializeField] private List<NoteTextEntry> noteTextEntries = new List<NoteTextEntry>();

    private Dictionary<int, TMP_Text> noteDictionary = new Dictionary<int, TMP_Text>();

    private void Awake()
    {
        for (int i = 0; i < noteTextEntries.Count; i++)
        {
            int id = noteTextEntries[i].noteID;
            TMP_Text text = noteTextEntries[i].noteText;

            if (!noteDictionary.ContainsKey(id))
            {
                noteDictionary.Add(id, text);
            }
        }
    }

    public override void Interact()
    {
        InteractableObject[] items = PlayerInventory.Instance.GetItemsForUI();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }

            int id = items[i].GetID();

            TMP_Text textToShow;

            if (noteDictionary.TryGetValue(id, out textToShow))
            {
                StartCoroutine(ShowTextForSeconds(textToShow));

                PlayerInventory.Instance.RemoveObject(items[i]);
                BookManager.Instance.AddNote();

                Debug.Log("Note archived: " + id);
                return;
            }
        }

        Debug.Log("No valid note found");
    }

    private IEnumerator ShowTextForSeconds(TMP_Text text)
    {
        text.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        text.gameObject.SetActive(false);
    }
}