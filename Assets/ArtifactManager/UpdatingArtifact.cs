using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatingArtifact : MonoBehaviour
{
    private GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArtifactInstantiate()
    {
        if (Player.hasDugged)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        Player.hasDugged = false;
    }
}
