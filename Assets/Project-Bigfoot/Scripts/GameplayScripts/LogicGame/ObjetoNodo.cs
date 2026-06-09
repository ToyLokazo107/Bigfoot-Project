using UnityEngine;

public class ObjetoNodo
{
    public InteractableObject DatosObjeto;
    public ObjetoNodo Siguiente;

    public ObjetoNodo(InteractableObject objeto)
    {
        DatosObjeto = objeto;
        Siguiente = null;
    }
}