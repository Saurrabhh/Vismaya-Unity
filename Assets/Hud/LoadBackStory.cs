using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBackStory : MonoBehaviour
{
    public GameObject DialogueCanvas;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DialogueCanvas.SetActive(true); 
            SceneManager.LoadScene((int)Scenes.Summary);
        }
    }
}
