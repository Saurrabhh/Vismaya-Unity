using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Artifact : MonoBehaviour

{
    public string artifactName;
    public int artifact_id;
    public GameObject m_gameObject;
    [SerializeField] public GameObject pillarArtifact;
    
    
    private void OnTriggerEnter(Collider other)
    {
        Player.hasDugged = true;
        Player.artifactsList.Add(this);
        
        Debug.Log("list entered");
        m_gameObject.SetActive(false);
        Player.expPoints += 100;
        
    }

    private void Start()
    {
        m_gameObject = this.gameObject;
    }

    

    public GameObject InstantiateOnPillar(GameObject prefab, Vector3 pos)
    {
        Vector3 finalPos;
        finalPos.x = pos.x;
        finalPos.y = pos.y + 1;
        finalPos.z = pos.z;
        GameObject a = Instantiate(prefab, finalPos, Quaternion.identity);
        a.gameObject.SetActive(true);
        return a;
    }

}
