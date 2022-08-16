using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBuy : MonoBehaviour
{
    [SerializeField] int itemCost;
    

    public void On_Click_Buy()
    {
        if(PlayerData.money >= itemCost)
        {
            PlayerData.money -= itemCost;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Limit Reached");
        }

        
    }
}
