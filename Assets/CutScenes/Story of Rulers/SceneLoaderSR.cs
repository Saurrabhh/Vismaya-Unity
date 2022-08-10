using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSR : MonoBehaviour
{
    int levelindex = 3;
    void Update()
    {
        SceneManager.LoadScene("Museum Scene", LoadSceneMode.Single);
    }
}
