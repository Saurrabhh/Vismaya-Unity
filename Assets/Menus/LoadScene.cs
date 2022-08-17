using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadingScene()
    {
        int scene = 5;
        SceneManager.LoadScene(scene);
    }
}
