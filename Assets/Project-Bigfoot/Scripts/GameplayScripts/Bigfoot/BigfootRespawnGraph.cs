using UnityEngine;
using UnityEngine.AI;
using Bigfoot.Collections.Graphs;

public class BigfootRespawnGraph : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private NonOrientedGraph<Transform> graph = new NonOrientedGraph<Transform>();

    private void Start()
    {
        CreateGraph();
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

        graph.PrintAdjancencyList();
    }

    public void RespawnFarFromPlayer()
    {
        Transform farthestPoint = GetFarthestPoint();

        if (farthestPoint == null) return;

        agent.enabled = false;
        transform.position = farthestPoint.position;
        agent.enabled = true;

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