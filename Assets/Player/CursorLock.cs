using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    bool hide = true;
    float lastStep, timeBetweenSteps = 0.5f;

    
    void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (Time.time - lastStep > timeBetweenSteps)
            {
                lastStep = Time.time;
                hide = !hide;
            }

            if (hide)
            {
                Cursor.visible = false;
            }
        }


        if (hide)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
        }
    }

    void lo()
    {
        hide = !hide;
        Debug.Log("jsec");
        new WaitForSeconds(1.0f);

    }
}
