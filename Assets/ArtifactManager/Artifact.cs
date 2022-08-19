using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Artifact : MonoBehaviour

{
    public string artifactName;
    public int artifact_id;
    public GameObject m_gameObject;
    public GameObject pillar;

   
    private void OnTriggerEnter(Collider other)
    {

        Player.artifactsList.Add(this);
        Destroy(gameObject);
        Debug.Log("list entered");


        Debug.Log("list ");

    }
}
