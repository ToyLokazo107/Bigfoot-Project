using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Range(0.0f, 24.0f)] public float Hora = 12;
    public Transform Sol;

    private float SolX;

    public float DuracionDiaMinutos = 1.0f;

    private void Update()
    {
        Hora += Time.deltaTime * (24 / (60* DuracionDiaMinutos));

        if(Hora >= 24)
        {
            Hora = 24;
        }

        RotacionSol();
    }

    void RotacionSol()
    {
        SolX = 15 * Hora;

        Sol.localEulerAngles = new Vector3(SolX, 0, 0);
    }


}
