using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBuy : MonoBehaviour
{
  
    public void On_Click_Buy()
    {
        Tool tool1 = GetComponent<Tool>();
        int itemCost = tool1.cost;
        Debug.Log(itemCost);
        if(Player.money >= itemCost)
        {
            Player.money -= itemCost;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Limit Reached");
        }

        
    }
}
