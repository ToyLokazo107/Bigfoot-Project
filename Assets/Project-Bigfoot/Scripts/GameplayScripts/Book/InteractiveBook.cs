using TMPro;
using UnityEngine;
using System.Collections;

public class InteractiveBook : InteractableObject
{
    public TMP_Text textoMateo;
    public TMP_Text textoLucia;
    public TMP_Text textoDiego;
    public TMP_Text textoCamila;
    public TMP_Text textoJavier;

    private IEnumerator OcultarTexto(TMP_Text texto)
    {
        yield return new WaitForSeconds(3f);

        texto.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        PlayerInventory inventario = FindFirstObjectByType<PlayerInventory>();

        InteractableObject[] objetos = inventario.ObtenerArregloParaUI();

        foreach (InteractableObject objeto in objetos)
        {
            if (objeto == null) continue;

            ItemRecolectable item = objeto.GetComponent<ItemRecolectable>();

            if (item == null || item.datosDelObjeto == null) continue;

            switch (item.datosDelObjeto.ID)
            {
                case 17:
                    textoMateo.gameObject.SetActive(true);
                    inventario.EliminarObjeto(objeto);
                    StartCoroutine(OcultarTexto(textoMateo));
                    BookManager.instance.AgregarNota();
                    Debug.Log("Nota Mateo colocada");
                    return;

                case 18:
                    textoLucia.gameObject.SetActive(true);
                    inventario.EliminarObjeto(objeto);
                    StartCoroutine(OcultarTexto(textoLucia));
                    BookManager.instance.AgregarNota();
                    Debug.Log("Nota Lucía colocada");
                    return;

                case 19:
                    textoDiego.gameObject.SetActive(true);
                    inventario.EliminarObjeto(objeto);
                    StartCoroutine(OcultarTexto(textoDiego));
                    BookManager.instance.AgregarNota();
                    Debug.Log("Nota Diego colocada");
                    return;

                case 20:
                    textoCamila.gameObject.SetActive(true);
                    inventario.EliminarObjeto(objeto);
                    StartCoroutine(OcultarTexto(textoCamila));
                    BookManager.instance.AgregarNota();
                    Debug.Log("Nota Camila colocada");
                    return;

                case 21:
                    textoJavier.gameObject.SetActive(true);
                    inventario.EliminarObjeto(objeto);
                    StartCoroutine(OcultarTexto(textoJavier));
                    BookManager.instance.AgregarNota();
                    Debug.Log("Nota Javier colocada");
                    return;
            }
        }

        Debug.Log("No tienes ninguna nota válida");
    }
}