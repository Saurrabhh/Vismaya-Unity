using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string name;
    public string uid;
    public string email;
    public int age;
    public string gender;
    public int currentSceneIndex;
    public static int money;

    public void SavePlayer()
    {
        SavePlayerData.SavePlayer(this);
    }

    private void Awake()
    {
        LoadPlayer();
    }

    public void LoadPlayer()
    {
        PlayerData playerData = SavePlayerData.LoadPlayer();
        name = playerData.name;
        age = playerData.age;
        uid = playerData.uid;
        email = playerData.email;
        gender = playerData.gender; 
        currentSceneIndex = playerData.currentSceneIndex;

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
}
