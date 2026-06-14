using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    private ObjetoNodo cabeza = null;
    private int cantidadActual = 0;
    private const int CAPACIDAD_MAXIMA = 3;

    public Action OnInventoryChanged;

    public bool AgregarObjeto(InteractableObject nuevoObjeto)
    {
        if (cantidadActual >= CAPACIDAD_MAXIMA)
        {
            Debug.Log("Inventario lleno");
            return false;
        }

        ObjetoNodo nuevoNodo = new ObjetoNodo(nuevoObjeto);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            ObjetoNodo actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo;
        }

        cantidadActual++;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public InteractableObject[] ObtenerArregloParaUI()
    {
        InteractableObject[] objetos = new InteractableObject[CAPACIDAD_MAXIMA];
        ObjetoNodo actual = cabeza;
        int indice = 0;

        while (actual != null && indice < CAPACIDAD_MAXIMA)
        {
            objetos[indice] = actual.DatosObjeto;
            actual = actual.Siguiente;
            indice++;
        }

        return objetos;
    }

    [Header("Slot System")]
    public int slotActivo = 0;

    public System.Action<int> OnSlotChanged;

    public void CambiarSlotActivo(int nuevoSlot)
    {
        slotActivo = nuevoSlot;
        Debug.Log("Cambiando al Slot: " + (slotActivo + 1));

        OnSlotChanged?.Invoke(slotActivo);

        ActualizarObjetosEnMano();
    }

    private void ActualizarObjetosEnMano()
    {
        GameObject puntoMano = GameObject.Find("ManoDerecha");
        if (puntoMano == null) return;

        foreach (Transform hijo in puntoMano.transform)
        {
            hijo.gameObject.SetActive(false);
        }

        InteractableObject[] lista = ObtenerArregloParaUI();

        if (slotActivo < lista.Length && lista[slotActivo] != null)
        {
            lista[slotActivo].gameObject.SetActive(true);
        }
    }

    public void SoltarObjetoActual()
    {
        InteractableObject[] lista = ObtenerArregloParaUI();

        if (slotActivo < lista.Length && lista[slotActivo] != null)
        {
            InteractableObject objetoASoltar = lista[slotActivo];

            ItemRecolectable itemComponent = objetoASoltar.GetComponent<ItemRecolectable>();
            if (itemComponent != null)
            {
                itemComponent.SoltarEnElSuelo();
            }

            cabeza = null;
            cantidadActual = 0;

            for (int i = 0; i < lista.Length; i++)
            {
                if (i != slotActivo && lista[i] != null)
                {
                    AgregarObjeto(lista[i]);
                }
            }

            ActualizarObjetosEnMano();
            OnInventoryChanged?.Invoke();
        }
    }
    public void EliminarObjeto(InteractableObject objeto)
    {
        if (cabeza == null) return;

        if (cabeza.DatosObjeto == objeto)
        {
            cabeza = cabeza.Siguiente;
            cantidadActual--;

            OnInventoryChanged?.Invoke();
            ActualizarObjetosEnMano();

            return;
        }

        ObjetoNodo actual = cabeza;

        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.DatosObjeto == objeto)
            {
                actual.Siguiente =
                    actual.Siguiente.Siguiente;

                cantidadActual--;

                OnInventoryChanged?.Invoke();
                ActualizarObjetosEnMano();

                return;
            }

            actual = actual.Siguiente;
        }
    }
}
