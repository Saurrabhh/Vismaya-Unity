using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadingScene()
    {
        int menuScene = 5;
        SceneManager.LoadScene(menuScene);
        
    }

    public void OpenSettings(Canvas settings)
    {
        GameObject hud = GameObject.FindGameObjectWithTag("HUD");
        settings.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        
    }
    public void OpenHUD(Canvas hud)
    {
        GameObject settings = GameObject.FindGameObjectWithTag("Settings");
        hud.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        
    }
}
