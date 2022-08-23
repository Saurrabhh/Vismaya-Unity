using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

   

   


}



