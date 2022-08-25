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
    public static bool hasDugged = false;
    public static int expPoints;

    public static List<Tool> totalTools = new();
    public static List<Tool> activeTools = new();
    public static List<Artifact> artifactsList = new();
    
    

   
    public void SavePlayer(string format)
    {
        //add scene index
        SavePlayerData.SavePlayer(this, format);
    }


    public void LoadPlayer(string uId, string format)
    {
        PlayerData playerData = SavePlayerData.LoadPlayer(uId, format);
        if(playerData == null)
        {
            return;
        }
        pName = playerData.name;
        age = playerData.age;
        uid = playerData.uid;
        email = playerData.email;
        gender = playerData.gender; 
        currentSceneIndex = playerData.currentSceneIndex;
        money = playerData.money;
        hasDugged = playerData.hasDugged;
        expPoints = playerData.expPoints;   
        transform.position = playerData.ReturnPosition();
        transform.rotation = playerData.ReturnRotation();
    }

   

   


}



