using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool IsInitialised { get; private set; }  //get anywhere but can't set from anywhere else
    public static GameObject mainMenu, artifactsMenu, optionsMenu, toolsMenu;
    public static void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        artifactsMenu = canvas.transform.Find("ArtifactsMenu").gameObject;
        optionsMenu = canvas.transform.Find("OptionsMenu").gameObject;
        toolsMenu = canvas.transform.Find("ToolsMenu").gameObject;
        IsInitialised = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu)
    {
        if(!IsInitialised)
            Init();


        switch (menu) {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case Menu.OPTIONS_MENU:
                optionsMenu.SetActive(true);
                break;
            case Menu.ARTIFACTS_MENU:
                artifactsMenu.SetActive(true);
                break;
            case Menu.TOOLS_MENU:
                toolsMenu.SetActive(true);
                break;
        
        }
        callingMenu.SetActive(false);

    }
}
