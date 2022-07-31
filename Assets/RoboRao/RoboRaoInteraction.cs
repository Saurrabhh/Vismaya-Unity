using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RoboRaoInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    [SerializeField]GameObject roboRaocanvas;
   

    private void OnCollisionEnter(Collision collision)
    {
        
        roboRaocanvas.SetActive(true);
        
    }


}
