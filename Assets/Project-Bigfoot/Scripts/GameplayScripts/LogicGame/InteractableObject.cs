using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public string objectName;
    public Sprite iconoObjeto;

    public abstract void Interact();

    public virtual void PrepareForInventory()
    {
    }

    public virtual void Use()
    {
    }

    public virtual void DropToGround()
    {
    }
    public virtual int GetID()
    {
        return -1;
    }
    public virtual string GetDescription()
    {
        return "";
    }
}