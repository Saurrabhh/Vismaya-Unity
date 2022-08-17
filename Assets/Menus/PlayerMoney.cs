using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI m_TextMeshPro;
    /*
    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
    }

    public void SubtractMoney(int moneyToSubtract)
    {
        if(money >= moneyToSubtract)
        {
            money -= moneyToSubtract;
        }
        else
        {
            Debug.Log("Limit Reached");
        }
    }
    */ 
    void Update()
    {
        m_TextMeshPro.text = Player.money.ToString();
    }
}
