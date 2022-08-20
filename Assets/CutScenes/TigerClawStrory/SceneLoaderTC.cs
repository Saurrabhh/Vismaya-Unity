using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTC : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene((int)Scenes.Terrain);
    }
}
