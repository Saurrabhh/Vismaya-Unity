using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static string pName;
    public static string uid;
    public static string email;
    public static int age;
    public static string gender;
    public static int currentSceneIndex;
    public static int money = 100;
    public static List<Tool> totalTools = new List<Tool>();
    public static List<Tool> activeTools = new List<Tool>();
    public static List<Artifact> artifactsList = new List<Artifact> ();
    public static List<Pillar> pillarInfo = new List<Pillar> ();
    public static bool hasDugged = false;

   
    public void SavePlayer()
    {
        //add scene index
        SavePlayerData.SavePlayer(this);
    }

    private void Awake()
    {
        //LoadPlayer();
    }

    public void LoadPlayer()
    {
        PlayerData playerData = SavePlayerData.LoadPlayer();
        pName = playerData.name;
        age = playerData.age;
        uid = playerData.uid;
        email = playerData.email;
        gender = playerData.gender; 
        currentSceneIndex = playerData.currentSceneIndex;
        money = playerData.money;

        transform.position = playerData.ReturnPosition();
        transform.rotation = playerData.ReturnRotation();
        Debug.Log(playerData.ReturnPosition());
    }

    private void Update()
    {
        
        if (hasDugged)
        {
            ArtifactInstantiate(artifactsList[0].m_gameObject, Vector3.positiveInfinity, pillarInfo[0]);
            hasDugged = false;
        }
         
    }

    public void ArtifactInstantiate(GameObject prefab, Vector3 pos, Pillar pillar)
    {
        Vector3 finalPosition;
        finalPosition.x = pillar.transform.position.x;
        finalPosition.y = pillar.transform.position.y + 1;
        finalPosition.z = pillar.transform.position.z;
        //Instantiate(prefab, pos, Quaternion.identity).gameObject.SetActive(true);
        prefab.transform.SetParent(pillar.transform);
        prefab.transform.position = finalPosition;
        prefab.gameObject.SetActive(true);
    }


}



