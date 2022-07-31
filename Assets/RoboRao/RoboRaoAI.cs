using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboRaoAI : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float offsetDistance = 1;
    [SerializeField] float interactDistance = 0.8f;
    [SerializeField] float offsetAngle = 45;
    [SerializeField] GameObject roboRaoCanvas;

    NavMeshAgent navMeshAgent;
    Vector3 offset;
    float distanceFromPlayer;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        float z_offset = Mathf.Cos(Mathf.Deg2Rad * (player.eulerAngles.y - offsetAngle)) * offsetDistance;
        float x_offset = Mathf.Sin(Mathf.Deg2Rad * (player.eulerAngles.y - offsetAngle)) * offsetDistance;
        offset = new Vector3(x_offset, 0, z_offset);

    }

   
    void Update()
    {
        FollowPlayer();
        InteracteWithPlayer();

    }
    private void FollowPlayer()
    {
        distanceFromPlayer = Vector3.Distance(navMeshAgent.transform.position, player.position);

        if (distanceFromPlayer > offsetDistance)
        {
            navMeshAgent.SetDestination(player.position + offset);
        }
    }
    private void InteracteWithPlayer()
    {
        if (distanceFromPlayer <= interactDistance)
        {
            roboRaoCanvas.SetActive(true);
        }
        else
        {
            roboRaoCanvas.SetActive(false);
        }
    }

    
}
