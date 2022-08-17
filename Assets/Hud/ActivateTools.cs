using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTools : MonoBehaviour
{
    public GameObject ToolsMenu;
    public void WhenButtonClicked()
    {
        if(ToolsMenu.activeInHierarchy == true)
        {
            ToolsMenu.SetActive(false); 
        }
        else
        {
            ToolsMenu.SetActive(true);
        }
    }
}
