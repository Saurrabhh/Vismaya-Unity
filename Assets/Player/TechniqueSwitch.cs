using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechniqueSwitch : MonoBehaviour
{
    Diggggg digger;
    void Start()
    {
        digger = GetComponent<Diggggg>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)){
            digger.size = 1f;
            digger.opacity = 2f;
            Debug.Log("Switched to Horizontal");
        }
        if (Input.GetKeyDown(KeyCode.H)){
            digger.size = 2f;
            digger.opacity = 0.8f;
            Debug.Log("Switched to Vertical");
        }
        
    }
}
