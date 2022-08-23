using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideronTrigger : MonoBehaviour
{
   [SerializeField] ParticleSystem artifactCollision;
    void Start()
    {
        artifactCollision = GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bagh")
        {
            artifactCollision.Play();
        }
    }
}
