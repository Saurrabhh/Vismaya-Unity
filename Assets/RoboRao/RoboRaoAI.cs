using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboRaoAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    [SerializeField] Vector3 offset = Vector3.zero;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

   
    void Update()
    {
       
        navMeshAgent.SetDestination(target.position);
        float distanceFromPlayer = Vector3.Distance(navMeshAgent.transform.position, target.position);
        if (distanceFromPlayer <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            //Move();
            //navMeshAgent.Move(offset);
            //navMeshAgent.transform.position = -target.position;
            Debug.Log("reached the player");
            
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    void Move()
    {
        float x = 1f;
        float y = navMeshAgent.transform.position.y;
        float z = 1f;
        Vector3 offset = new Vector3(x, y, z);
        navMeshAgent.transform.position = target.position + offset;
    }
    


}
