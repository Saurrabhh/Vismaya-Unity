using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class ToggleHUD : MonoBehaviour
{
    public GameObject hud;
    public GameObject player;
    public GameObject chat;
    public GameObject settings;

    private void Awake()
    {
        hud = GameObject.FindGameObjectWithTag("HUD");
        player = FindObjectOfType<Player>().gameObject;
        chat = GameObject.FindGameObjectWithTag("Chat");
        settings = GameObject.FindGameObjectWithTag("Settings");
    }

    public void OpenSettings(Canvas settings)
    {
        
        settings.gameObject.SetActive(true);
        settings.GetComponent<Canvas>().enabled = true;
        hud.gameObject.SetActive(false);
        player.GetComponent<ThirdPersonController>().enabled = false;
        
    }
public void OpenChat(Canvas chat)
    {
        chat.GetComponent<Canvas>().enabled = true;
        hud.gameObject.SetActive(false);
        player.GetComponent<ThirdPersonController>().enabled = false;

    }
    public void OpenHUD(Canvas hud)
    {

        StartCoroutine(OpenHUDRoutine(hud));
        
    }
  public void OpenHUDfromChat(Canvas hud)
    {
        chat = GameObject.FindGameObjectWithTag("Chat");
        chat.GetComponent<Canvas>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;
        hud.gameObject.SetActive(true);


    }

    IEnumerator OpenHUDRoutine(Canvas hud)
    {

        settings = GameObject.FindGameObjectWithTag("Settings");
        settings.GetComponent<Canvas>().enabled = false;        
        hud.gameObject.SetActive(true);
        player.GetComponent<ThirdPersonController>().enabled = true;
        yield return null;
    }
}
