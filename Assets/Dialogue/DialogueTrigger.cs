using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public Avatar[] avatars;
    
    [SerializeField] DialogueManager manager;
    public GameObject canvas;
    public void StartDialogue()  
    {
        manager.gameObject.SetActive(true);
        manager.OpenDialogue(dialogues, avatars);
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("ggg");
            StartDialogue();
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
}
[System.Serializable]
public class Dialogue
{
    public int AvatarId;
    public string dialogue;
}
[System.Serializable]
public class Avatar 
{ 
    public string name;
    public Sprite sprite;

}

