using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private ObjetoNodo head = null;
    private int currentAmount = 0;

    [Header("Inventory Settings")]
    [SerializeField] private int maxCapacity = 3;

    [Header("Slot System")]
    public int activeSlot = 0;

    public Action OnInventoryChanged;
    public Action<int> OnSlotChanged;

    private void Awake()
    {
        Instance = this;
    }

    public bool AddObject(InteractableObject newObject)
    {
        if (currentAmount >= maxCapacity)
        {
            Debug.Log("Inventory full");
            return false;
        }

        ObjetoNodo newNode = new ObjetoNodo(newObject);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            ObjetoNodo current = head;

            while (current.Siguiente != null)
            {
                current = current.Siguiente;
            }

            current.Siguiente = newNode;
        }

        currentAmount++;
        OnInventoryChanged?.Invoke();

        return true;
    }

    public InteractableObject[] GetItemsForUI()
    {
        InteractableObject[] items = new InteractableObject[maxCapacity];

        ObjetoNodo current = head;
        int index = 0;

        while (current != null && index < maxCapacity)
        {
            items[index] = current.DatosObjeto;
            current = current.Siguiente;
            index++;
        }

        return items;
    }

    public void ChangeActiveSlot(int newSlot)
    {
        if (newSlot < 0 || newSlot >= maxCapacity) return;

        activeSlot = newSlot;

        Debug.Log("Changing to Slot: " + (activeSlot + 1));

        OnSlotChanged?.Invoke(activeSlot);

        PlayerHandController.Instance.EquipObject(GetCurrentObject());
    }

    public InteractableObject GetCurrentObject()
    {
        InteractableObject[] items = GetItemsForUI();

        if (activeSlot < items.Length)
        {
            return items[activeSlot];
        }

        return null;
    }

    public void DropCurrentObject()
    {
        InteractableObject currentObject = GetCurrentObject();

        if (currentObject == null) return;

        PlayerHandController.Instance.DropObject(currentObject);
        RemoveObject(currentObject);
    }

    public void RemoveObject(InteractableObject objectToRemove)
    {
        if (head == null || objectToRemove == null) return;

        if (head.DatosObjeto == objectToRemove)
        {
            head = head.Siguiente;
            currentAmount--;

            OnInventoryChanged?.Invoke();

            PlayerHandController.Instance.EquipObject(GetCurrentObject());

            return;
        }

        ObjetoNodo current = head;

        while (current.Siguiente != null)
        {
            if (current.Siguiente.DatosObjeto == objectToRemove)
            {
                current.Siguiente = current.Siguiente.Siguiente;
                currentAmount--;

                OnInventoryChanged?.Invoke();

                PlayerHandController.Instance.EquipObject(GetCurrentObject());

                return;
            }

            current = current.Siguiente;
        }
    }
}
