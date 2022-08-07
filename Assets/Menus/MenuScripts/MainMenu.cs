using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public void OnClick_Artifacts()
   {
        MenuManager.OpenMenu(Menu.ARTIFACTS_MENU, gameObject);
   }

   public void OnClick_Options()
   {
        MenuManager.OpenMenu(Menu.OPTIONS_MENU, gameObject);
    }
   public void OnClick_Tools()
   {
        MenuManager.OpenMenu(Menu.TOOLS_MENU, gameObject);
    }
}
