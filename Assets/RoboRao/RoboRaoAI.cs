using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboRaoAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

   
    void Update()
    {
       
        navMeshAgent.SetDestination(target.position);
        float distanceFromPlayer = Vector3.Distance(navMeshAgent.transform.position, target.position);
        if (distanceFromPlayer < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            RoboraoIsStationary();
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    bool RoboraoIsStationary()
    {
        if (this.rb.velocity == Vector3.zero)
        {
            Debug.Log("reached the player");
            return true;
        }
        return false;
    }

}
