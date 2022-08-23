using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCanvas : MonoBehaviour
{
    public void OnClick_Battlefield()
    {
        SceneManager.LoadScene((int)Scenes.Terrain);
    }

    public void OnClick_Entertainment()
    {
        Debug.Log("Entertainment tp");
    }

    public void OnClick_Business()
    {
        Debug.Log("Business tp");
    }
}
