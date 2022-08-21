using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    GameObject player;
    public static bool isPaused = false;
    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }


        if (isPaused)
        {
            OpenSettings();
        }
        else
        {
            ExitSettings();
        }
    }

    private void ExitSettings()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        Time.timeScale = 0f;
       
    }
}
