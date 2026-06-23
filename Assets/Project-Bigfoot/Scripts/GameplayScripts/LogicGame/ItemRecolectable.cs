using UnityEngine;

public class ItemRecolectable : InteractableObject
{
    public ObjectsData datosDelObjeto;

    [Header("Flashlight Settings")]
    public Light luzLinterna;

    private bool isOn = false;
    private bool isEquipped = false;
    private Vector3 originalScale;

    private void Start()
    {
        objectName = datosDelObjeto.objectName;
        iconoObjeto = datosDelObjeto.Icon;

        originalScale = transform.lossyScale;

        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
        }
    }

    public override int GetID()
    {
        return datosDelObjeto.ID;
    }

    public override string GetDescription()
    {
        return datosDelObjeto.Description;
    }

    public override void Interact()
    {
        if (PlayerInventory.Instance == null) return;

        bool added = PlayerInventory.Instance.AddObject(this);

        if (added)
        {
            PlayerHandController.Instance.EquipObject(this);
        }
    }

    public override void PrepareForInventory()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        isEquipped = true;
    }

    public override void Use()
    {
        IUsableItem usableItem = GetComponent<IUsableItem>();

        if (usableItem != null)
        {
            usableItem.UseItem();
            return;
        }

        if (!isEquipped || luzLinterna == null) return;

        isOn = !isOn;
        luzLinterna.enabled = isOn;
    }

    public override void DropToGround()
    {
        transform.SetParent(null);

        transform.localScale = originalScale;
        transform.rotation = Quaternion.identity;
        transform.position += new Vector3(0f, 0.3f, 0f);

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        isEquipped = false;
    }
}