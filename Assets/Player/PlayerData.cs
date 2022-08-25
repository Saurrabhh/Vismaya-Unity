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
    public int money;
    public bool hasDugged;
    public int expPoints;

    public PlayerData(Player player)
    {
        this.name = Player.pName;
        this.email = Player.email;
        this.currentSceneIndex = Player.currentSceneIndex;
        this.age = Player.age;
        this.gender = Player.gender;
        this.uid = Player.uid;
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        this.position = new float[3] { pos.x, pos.y, pos.z };
        this.rotation = new float[4] { rot.x, rot.y, rot.z, rot.w };
        this.money = Player.money;
        this.hasDugged = Player.hasDugged;
        this.expPoints = Player.expPoints;
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
