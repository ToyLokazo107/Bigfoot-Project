using UnityEngine;

public class ItemRecolectable : InteractableObject
{
    [Header("Flashlight Settings")]
    public Light luzLinterna;
    private bool estaEncendida = false;
    private bool estaEquipada = false;

    private void Start()
    {
        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
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

            transform.SetParent(puntoMano.transform);
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
        estaEquipada = false;

        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
            estaEncendida = false;
        }

        transform.SetParent(null);

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            transform.position = jugador.transform.position + jugador.transform.forward * 1.5f + Vector3.up * 0.5f;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            if (jugador != null)
            {
                rb.AddForce(jugador.transform.forward * 2f, ForceMode.Impulse);
            }
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        gameObject.SetActive(true);
    }
}