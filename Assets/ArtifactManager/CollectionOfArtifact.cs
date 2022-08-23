using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionOfArtifact : MonoBehaviour
{
    public List<Artifact> allArtifacts;
    List<Artifact> activeArtifacts;
    // Start is called before the first frame update
    void Start()
    {
        activeArtifacts = Player.artifactsList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.hasDugged)
        {
            CheckName();
            Player.hasDugged = false;
        }
        
    }

    public void CheckName()
    {
       
        foreach (var allArtifact in allArtifacts)
        {

            foreach(var activeArtifact in activeArtifacts)
            if (allArtifact.artifactName == activeArtifact.artifactName)
            {
                //instantiate 
                GameObject artifact = activeArtifact.InstantiateOnPillar(allArtifact.m_gameObject, allArtifact.pillarArtifact.transform.position);
                artifact.transform.SetParent(allArtifact.pillarArtifact.transform);

            }
            else
            {
                continue;
            }
        }
    }
}
