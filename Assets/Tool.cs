using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tool : MonoBehaviour
{
    public string id;
    public int cost;
    public Image img;
    private void Awake()
    {
        id = transform.name;
        Image[] imges = GetComponentsInChildren<Image>();
        img = imges[1];
    }

    public void On_Click_Buy()
    {
        Transform newParent = GameObject.FindGameObjectWithTag("Owned").transform;
        
        if (Player.money >= cost)
        {
            Player.money -= cost;
            Destroy(gameObject.GetComponentInChildren<Button>().gameObject);
            gameObject.transform.SetParent(newParent);
            Player.totalTools.Add(this);
        }
        else
        {
            Debug.Log("Limit Reached");
        }


    }


}
