using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboRaoAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    [SerializeField] int offset = 1;
    public GameObject roboRaocanvas;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
       
    }

   
    void Update()
    {
        float z_offset = Mathf.Cos(Mathf.Deg2Rad * (target.eulerAngles.y - 45)) * offset;
        float x_offset = Mathf.Sin(Mathf.Deg2Rad * (target.eulerAngles.y - 45)) * offset;
        Debug.Log(x_offset + " " + z_offset);
        Vector3 off = new Vector3(x_offset, 0, z_offset);


        //navMeshAgent.SetDestination(target.position + off);

        float distanceFromPlayer = Vector3.Distance(navMeshAgent.transform.position, target.position);
        if (distanceFromPlayer <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            Debug.Log("reached the player");
            
        }
        else
        {
            navMeshAgent.isStopped = false;

            
            Debug.Log("Not reached the player");


            navMeshAgent.SetDestination(target.position + off);

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
