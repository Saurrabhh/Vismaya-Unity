using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public Vector3 pillarPosition;
    public bool hasArtifact = false;
    //public static List<Vector3> pillarPosList = new List<Vector3>();
    [SerializeField]public GameObject pillar;
    public Artifact artifact;

    private void Start()
    {
        AppendPosition();
        
    }
    public void AppendPosition()
    {

        Player.pillarInfo.Add(this);
        
       // pillarPosition = pillar.transform.position;
        
       // pillarPosList.Add(pillarPosition);
       // for(int i = 0; i < pillarPosList.Count; i++)
        //{
        //    Debug.Log(pillarPosList[0]);
        //}
    }

   

    private void Update()
    {
        if (Player.hasDugged)
        {
            return;
        }
    }

}
