using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public static PlayerHandController Instance;

    [Header("References")]
    [SerializeField] private Transform rightHandPoint;

    private InteractableObject currentObject;

    private void Awake()
    {
        Instance = this;
    }

    public void EquipObject(InteractableObject objectToEquip)
    {
        HideCurrentObject();

        if (objectToEquip == null)
        {
            currentObject = null;
            return;
        }

        objectToEquip.PrepareForInventory();

        objectToEquip.transform.SetParent(rightHandPoint);
        objectToEquip.transform.localPosition = Vector3.zero;
        objectToEquip.transform.localRotation = Quaternion.Euler(-0.158f, 0f, 0f);
        objectToEquip.gameObject.SetActive(true);

        currentObject = objectToEquip;
    }

    public void HideCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.SetActive(false);
        }
    }

    public void UseCurrentObject()
    {
        if (currentObject != null)
        {
            currentObject.Use();
        }
    }

    public void DropObject(InteractableObject objectToDrop)
    {
        if (objectToDrop == null) return;

        objectToDrop.DropToGround();

        if (currentObject == objectToDrop)
        {
            currentObject = null;
        }
    }
}