using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Scenes scene;
    public bool open;
    public void LoadScene()
    {
        SceneManager.LoadScene((int)scene);
    }

    private void Update()
    {
        if (open)
        {
            LoadScene();
        }
    }
}
