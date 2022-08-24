using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Explore : MonoBehaviour
{
    public GameObject roboRao;
    public GameObject player;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        roboRao.GetComponent<RoboRaoAI>().enabled = false;
        roboRao.GetComponent<NavMeshAgent>().enabled = false;
        player.GetComponent<Diggggg>().enabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        roboRao.GetComponent<RoboRaoAI>().enabled = true;
        roboRao.GetComponent<NavMeshAgent>().enabled = true;
        player.GetComponent<Diggggg>().enabled = false;
    }
}
