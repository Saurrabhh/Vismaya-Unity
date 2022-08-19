using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBackStory : MonoBehaviour
{
    public GameObject canvas;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        { 
            Debug.Log("Lalala");
            SceneManager.LoadScene((int)Scenes.Summary);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }
}
