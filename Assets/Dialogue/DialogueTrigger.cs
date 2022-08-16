using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public Avatar[] avatars;
    
    [SerializeField] DialogueManager manager;
    public void StartDialogue()  
    {
        manager.OpenDialogue(dialogues, avatars);
       
    }
    private void OnTriggerEnter(Collider other)
    {
        StartDialogue();
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

