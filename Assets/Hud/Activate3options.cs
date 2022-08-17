using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate3options : MonoBehaviour
{
    public GameObject ThreeOptionsCanvas;
    public void WhenButtonClicked()
    {
        if (ThreeOptionsCanvas.activeInHierarchy == true)
        {
            ThreeOptionsCanvas.SetActive(false);
        }
        else
        {
            ThreeOptionsCanvas.SetActive(true);
        }
    }
}
