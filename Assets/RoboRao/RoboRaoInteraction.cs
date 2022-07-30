using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RoboRaoInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    GameObject roboRaocanvas;
    void Start()
    {
        roboRaocanvas = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        roboRaocanvas.SetActive(true);
        
    }


}
