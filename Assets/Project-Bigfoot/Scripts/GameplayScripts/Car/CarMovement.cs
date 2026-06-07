using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform destino;
    public float velocidad = 5f;

    public float distanciaMinima = 0.2f;

    void Update()
    {
        if (destino == null) return;

        Vector3 posicionCocheHorizontal = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posicionDestinoHorizontal = new Vector3(destino.position.x, 0, destino.position.z);

        float distanciaAlObjetivo = Vector3.Distance(posicionCocheHorizontal, posicionDestinoHorizontal);

        if (distanciaAlObjetivo <= distanciaMinima)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        Vector3 direccion = destino.position - transform.position;
        direccion.y = 0;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = rotacionObjetivo;
        }
    }
}