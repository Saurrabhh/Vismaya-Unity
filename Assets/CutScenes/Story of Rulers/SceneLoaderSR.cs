using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSR : MonoBehaviour
{
    int levelindex = 2;
    void Update()
    {
        SceneManager.LoadScene(levelindex, LoadSceneMode.Single);
    }
}
