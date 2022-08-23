using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideronTrigger : MonoBehaviour
{
   
   [SerializeField] ParticleSystem artifactCollision;
    void OnTriggerEnter(Collider other)
    {
        artifactCollision.Play();
    }
    
}
