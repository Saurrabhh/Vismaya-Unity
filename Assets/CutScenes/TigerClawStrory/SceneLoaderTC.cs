using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTC : MonoBehaviour
{
    int levelIndex = 1;
    void Update()
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
}
