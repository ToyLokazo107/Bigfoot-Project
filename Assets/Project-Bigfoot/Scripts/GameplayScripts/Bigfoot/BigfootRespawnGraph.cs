using UnityEngine;
using UnityEngine.AI;
using Bigfoot.Collections.Graphs;
using Unity.Cinemachine;
using System.Collections;

public class BigfootRespawnGraph : MonoBehaviour
{
    private enum EstadoEnemigo { Rondando, Persiguiendo, Huyendo }
    [SerializeField]private EstadoEnemigo estadoActual = EstadoEnemigo.Rondando;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Patrol Settings")]
    [Tooltip("Distancia mínima para considerar que llegó al punto de patrulla")]
    public float stoppingDistanceNode = 2.0f;

    [Header("Velocidades del Bigfoot")]
    public float velocidadRondando = 5.5f;
    public float velocidadPersiguiendo = 8.5f;
    public float velocidadHuyendo = 12f;

    [Header("Configuración de Ataque Sorpresa")]
    public float tiempoMinimoParaAtacar = 24f;
    public float tiempoMaximoParaAtacar = 30f;

    private NonOrientedGraph<Transform> graph = new NonOrientedGraph<Transform>();
    private Node<Transform> nodoDestinoActual;
    private float temporizadorAtaqueRandom;

    public Animator animator;


    private void Start()
    {
        CreateGraph();

        agent.speed = velocidadRondando;

        ReiniciarTemporizadorAtaque();

        if (graph.Nodes.Count > 0)
        {
            nodoDestinoActual = graph.Nodes[0];
            IrANodoActual();
        }
        else
        {
            if (player != null) agent.SetDestination(player.transform.position);
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
        {
            if (agent.hasPath) agent.ResetPath();
            return;
        }

        switch (estadoActual)
        {
            case EstadoEnemigo.Rondando:
                LógicaRondar();
                ControlarTemporizadorAtaque();
                break;

            case EstadoEnemigo.Persiguiendo:
                LógicaPersiguiendo();
                break;

            case EstadoEnemigo.Huyendo:
                LógicaHuyendo();
                break;
        }
    }

    private IEnumerator AttackPlayer()
    {
        agent.isStopped = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.6f);

        Debug.Log("Bigfoot golpeó al jugador");

        yield return new WaitForSeconds(0.4f);

        agent.isStopped = false;

        HuirAlNodoMasLejano();
    }

    private void LógicaRondar()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistanceNode)
        {
            CambiarANextNodoRondando();
        }
    }

    private void ControlarTemporizadorAtaque()
    {
        if (player == null) return;

        temporizadorAtaqueRandom -= Time.deltaTime;

        if (temporizadorAtaqueRandom <= 0f)
        {
            IniciarPersecucionSorpresa();
        }
    }

    private void IniciarPersecucionSorpresa()
    {
        estadoActual = EstadoEnemigo.Persiguiendo;
        agent.speed = velocidadPersiguiendo;
        Debug.Log("¡El Bigfoot te ha detectado y empezó a correr hacia ti!");
    }

    private void LógicaPersiguiendo()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
    }

    private void LógicaHuyendo()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistanceNode)
        {
            estadoActual = EstadoEnemigo.Rondando;
            agent.speed = velocidadRondando;
            ReiniciarTemporizadorAtaque();
            CambiarANextNodoRondando();
            Debug.Log("El Bigfoot se ha calmado y vuelve a rondar los nodos.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (estadoActual == EstadoEnemigo.Persiguiendo && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡El Bigfoot golpeó al jugador!");

            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);

            HuirAlNodoMasLejano();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (estadoActual == EstadoEnemigo.Persiguiendo && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡El Bigfoot alcanzó al jugador!");

            StartCoroutine(AttackPlayer());
        }
    }
    private void HuirAlNodoMasLejano()
    {
        Transform farthestPoint = GetFarthestPoint();

        if (farthestPoint == null) return;

        estadoActual = EstadoEnemigo.Huyendo;
        agent.speed = velocidadHuyendo;

        foreach (var node in graph.Nodes)
        {
            if (node.Value == farthestPoint)
            {
                nodoDestinoActual = node;
                break;
            }
        }

        agent.SetDestination(farthestPoint.position);
        Debug.Log("¡El Bigfoot está huyendo rápidamente hacia: " + farthestPoint.name);
    }

    private void ReiniciarTemporizadorAtaque()
    {
        temporizadorAtaqueRandom = Random.Range(tiempoMinimoParaAtacar, tiempoMaximoParaAtacar);
    }

    private void CreateGraph()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            graph.AddNode(spawnPoints[i]);
        }

        for (int i = 0; i < graph.Nodes.Count - 1; i++)
        {
            graph.AddEdges(i, i + 1);
        }

        if (graph.Nodes.Count > 2)
        {
            graph.AddEdges(graph.Nodes.Count - 1, 0);
        }

        graph.PrintAdjancencyList();
    }

    private void IrANodoActual()
    {
        if (nodoDestinoActual != null && nodoDestinoActual.Value != null)
        {
            agent.SetDestination(nodoDestinoActual.Value.position);
        }
    }

    private void CambiarANextNodoRondando()
    {
        if (nodoDestinoActual == null || nodoDestinoActual.Neighbors.Count == 0) return;

        int indiceAleatorio = Random.Range(0, nodoDestinoActual.Neighbors.Count);
        nodoDestinoActual = nodoDestinoActual.Neighbors[indiceAleatorio];

        IrANodoActual();
    }

    public void RespawnFarFromPlayer()
    {
        Transform farthestPoint = GetFarthestPoint();

        if (farthestPoint == null) return;

        agent.enabled = false;
        transform.position = farthestPoint.position;
        agent.enabled = true;

        foreach (var node in graph.Nodes)
        {
            if (node.Value == farthestPoint)
            {
                nodoDestinoActual = node;
                break;
            }
        }

        estadoActual = EstadoEnemigo.Rondando;
        agent.speed = velocidadRondando;
        ReiniciarTemporizadorAtaque();
        IrANodoActual();
        Debug.Log("Bigfoot respawned at: " + farthestPoint.name);
    }

    private Transform GetFarthestPoint()
    {
        Transform farthest = null;
        float maxDistance = 0f;

        for (int i = 0; i < graph.Nodes.Count; i++)
        {
            Transform point = graph.Nodes[i].Value;
            float distance = Vector3.Distance(player.position, point.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthest = point;
            }
        }

        return farthest;
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] != null)
            {
                Gizmos.DrawSphere(spawnPoints[i].position, 0.5f);

                if (i < spawnPoints.Length - 1 && spawnPoints[i + 1] != null)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(spawnPoints[i].position, spawnPoints[i + 1].position);
                    Gizmos.color = Color.cyan;
                }
            }
        }

        if (spawnPoints.Length > 2 && spawnPoints[0] != null && spawnPoints[spawnPoints.Length - 1] != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(spawnPoints[spawnPoints.Length - 1].position, spawnPoints[0].position);
        }
    }
}

