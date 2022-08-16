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
    public static int money;
    public static int[] tools;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("is");
        }
    }

    void UpdateUser()
    {

    }
}
