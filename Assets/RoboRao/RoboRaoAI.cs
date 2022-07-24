using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboRaoAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
   
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

   
    void Update()
    {
       
        navMeshAgent.SetDestination(target.position);
        float distanceFromPlayer = Vector3.Distance(navMeshAgent.transform.position, target.position);
        if (distanceFromPlayer < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            Debug.Log("reached the player");
            
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    

}
