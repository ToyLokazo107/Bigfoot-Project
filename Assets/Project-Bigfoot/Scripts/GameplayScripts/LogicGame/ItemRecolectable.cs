using UnityEngine;

public class ItemRecolectable : InteractableObject
{
    public ObjectsData datosDelObjeto;

    [Header("Flashlight Settings")]
    public Light luzLinterna;
    private bool estaEncendida = false;
    private bool estaEquipada = false;
    private Vector3 escalaOriginalMundo;

    private void Start()
    {
        objectName = datosDelObjeto.objectName;
        iconoObjeto = datosDelObjeto.Icon;

        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
            escalaOriginalMundo = transform.lossyScale;
        }
        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
            escalaOriginalMundo = transform.lossyScale;
        }
    }

    public override void Interact()
    {
        PlayerInventory inventario = FindFirstObjectByType<PlayerInventory>();

        if (inventario != null)
        {
            bool exito = inventario.AgregarObjeto(this);
            if (exito)
            {
                EquiparEnMano();
            }
        }
    }

    private void EquiparEnMano()
    {
        GameObject puntoMano = GameObject.Find("ManoDerecha");

        if (puntoMano != null)
        {
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            if (GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = false;
            }

            transform.SetParent(puntoMano.transform, true);
            transform.localPosition = Vector3.zero;

            transform.localRotation = Quaternion.Euler(-0.158f, 0f, 0f);

            estaEquipada = true;
            gameObject.SetActive(true);
        }
    }

    public void AlternarUso()
    {
        if (!estaEquipada || luzLinterna == null) return;

        estaEncendida = !estaEncendida;
        luzLinterna.enabled = estaEncendida;

    }

    public void SoltarEnElSuelo()
    {
        transform.SetParent(null);

        transform.localScale = escalaOriginalMundo;
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
    }
}