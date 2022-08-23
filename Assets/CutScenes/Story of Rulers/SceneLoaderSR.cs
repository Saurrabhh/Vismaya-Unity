using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderSR : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene((int)Scenes.Museum);
    }
}
