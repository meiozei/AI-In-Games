using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    public ObstacleAvoidanceType AvoidanceType;
    public float AvoidancePredictionTime;
    public float AgentSpeed;
    public Transform Destination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NavMesh.avoidancePredictionTime = AvoidancePredictionTime;
        agent.obstacleAvoidanceType = AvoidanceType;
        agent.speed = AgentSpeed;
        agent.SetDestination(Destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
