using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public Image[] slotImages;
    public Sprite emptySlotSprite;

    private void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += UpdateUI;
        }

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateUI;
        }
    }

    public void UpdateUI()
    {
        if (playerInventory == null) return;

        InteractableObject[] itemList = playerInventory.GetItemsForUI();

        for (int i = 0; i < slotImages.Length; i++)
        {
            if (i < itemList.Length && itemList[i] != null)
            {
                slotImages[i].sprite = itemList[i].iconoObjeto;
                slotImages[i].color = Color.white;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
                slotImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
}