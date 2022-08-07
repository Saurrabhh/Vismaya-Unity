using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public string uid;
    public string email;
    public int age;
    public string gender;
    public float[] position;
    public float[] rotation;
    public int currentSceneIndex;

    public PlayerData(Player player)
    {
        this.name = player.name;
        this.email = player.email;
        this.currentSceneIndex = player.currentSceneIndex;
        this.age = player.age;
        this.gender = player.gender;
        this.uid = player.uid;
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        this.position = new float[3] { pos.x, pos.y, pos.z };
        this.rotation = new float[4] { rot.x, rot.y, rot.z, rot.w };
    }

    public Vector3 ReturnPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
    
    public Quaternion ReturnRotation()
    {
        return new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
    }
}
