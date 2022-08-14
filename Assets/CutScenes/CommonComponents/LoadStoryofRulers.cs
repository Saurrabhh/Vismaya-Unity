using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStoryofRulers : MonoBehaviour
{
    public void LoadScene()
    {
        int loadlevel = 3;
        SceneManager.LoadScene(loadlevel, LoadSceneMode.Single);
    }
    
}
