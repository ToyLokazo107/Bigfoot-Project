using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public Image[] slotsImagenes;
    public Sprite slotVacioSprite;

    private void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += ActualizarInterfaz;
        }
        ActualizarInterfaz();
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= ActualizarInterfaz;
        }
    }

    public void ActualizarInterfaz()
    {
        if (playerInventory == null) return;

        InteractableObject[] listaObjetos = playerInventory.ObtenerArregloParaUI();

        for (int i = 0; i < slotsImagenes.Length; i++)
        {
            if (i < listaObjetos.Length && listaObjetos[i] != null)
            {
                slotsImagenes[i].sprite = listaObjetos[i].iconoObjeto;
                slotsImagenes[i].color = Color.white;
            }
            else
            {
                slotsImagenes[i].sprite = slotVacioSprite;
                slotsImagenes[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
}