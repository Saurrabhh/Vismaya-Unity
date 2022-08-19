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
        Player.hasDugged = true;
        Player.artifactsList.Add(this);
        m_gameObject.SetActive(false);
        Debug.Log("list entered");


        Debug.Log("list ");

    }
}
