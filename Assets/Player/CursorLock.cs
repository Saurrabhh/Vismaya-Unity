using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    bool hide = true;
    float lastStep, timeBetweenSteps = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Escape)){
            if (Time.time - lastStep > timeBetweenSteps)
            {
                lastStep = Time.time;
                hide = !hide;
                Debug.Log("kkkkk");
            }
        }


        if (hide)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    void lo()
    {
        hide = !hide;
        Debug.Log("jsec");
        new WaitForSeconds(1.0f);

    }
}
