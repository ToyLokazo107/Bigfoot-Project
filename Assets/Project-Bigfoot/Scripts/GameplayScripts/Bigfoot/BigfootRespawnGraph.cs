using UnityEngine;
using UnityEngine.AI;
using Bigfoot.Collections.Graphs;

public class BigfootRespawnGraph : MonoBehaviour
{
    private enum EstadoEnemigo { Rondando, Persiguiendo, Huyendo }
    private EstadoEnemigo estadoActual = EstadoEnemigo.Rondando;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Patrol Settings")]
    [Tooltip("Distancia mínima para considerar que llegó al punto de patrulla")]
    public float stoppingDistanceNode = 2.0f;

    [Header("Velocidades del Bigfoot")]
    public float velocidadRondando = 3.5f;
    public float velocidadPersiguiendo = 7f;
    public float velocidadHuyendo = 9f;

    [Header("Configuración de Ataque Sorpresa")]
    public float tiempoMinimoParaAtacar = 10f;
    public float tiempoMaximoParaAtacar = 25f;

    private NonOrientedGraph<Transform> graph = new NonOrientedGraph<Transform>();
    private Node<Transform> nodoDestinoActual;
    private float temporizadorAtaqueRandom;

    private void Start()
    {
        CreateGraph();

        // Configurar velocidad inicial
        agent.speed = velocidadRondando;

        // Establecer el primer tiempo aleatorio para el ataque sorpresa
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
        // Si el juego no está en cacería o el agente no está activo, detenemos la lógica
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
        {
            if (agent.hasPath) agent.ResetPath();
            return;
        }

        // CONTROLADOR DE ESTADOS
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

    private void LógicaRondar()
    {
        // Si llegó al nodo actual de patrulla, cambia al siguiente
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistanceNode)
        {
            CambiarANextNodoRondando();
        }
    }

    private void ControlarTemporizadorAtaque()
    {
        if (player == null) return;

        temporizadorAtaqueRandom -= Time.deltaTime;

        // ¡Llegó el momento de atacar por sorpresa!
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

        // Actualiza constantemente la posición del jugador para cazarlo
        agent.SetDestination(player.position);
    }

    private void LógicaHuyendo()
    {
        // Al huir, revisamos si ya llegó con éxito al nodo más lejano asignado
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistanceNode)
        {
            // Una vez a salvo en el nodo lejano, vuelve a patrullar con normalidad
            estadoActual = EstadoEnemigo.Rondando;
            agent.speed = velocidadRondando;
            ReiniciarTemporizadorAtaque();
            CambiarANextNodoRondando();
            Debug.Log("El Bigfoot se ha calmado y vuelve a rondar los nodos.");
        }
    }

    // DETECCIÓN DE COLISIÓN CON EL PLAYER
    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si chocamos contra el jugador mientras lo perseguíamos
        if (estadoActual == EstadoEnemigo.Persiguiendo && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¡El Bigfoot golpeó al jugador!");

            // Aquí puedes llamar al sistema de daño del jugador si tienes uno, ej:
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);

            HuirAlNodoMasLejano();
        }
    }

    private void HuirAlNodoMasLejano()
    {
        Transform farthestPoint = GetFarthestPoint();

        if (farthestPoint == null) return;

        estadoActual = EstadoEnemigo.Huyendo;
        agent.speed = velocidadHuyendo; // Cambia su velocidad a modo Huida (más rápido)

        // Sincronizamos nuestro sistema de grafos con este punto de escape
        foreach (var node in graph.Nodes)
        {
            if (node.Value == farthestPoint)
            {
                nodoDestinoActual = node;
                break;
            }
        }

        // Le ordenamos al NavMeshAgent correr hacia allá de inmediato
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
}