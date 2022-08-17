using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTools : MonoBehaviour
{
    public GameObject Tools;
    public void WhenButtonClicked()
    {
        if(Tools.activeInHierarchy == true)
        {
            Tools.SetActive(false); 
        }
        else
        {
            Tools.SetActive(true);  
        }
    }
}
