using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public Avatar[] avatars;
    [SerializeField] DialogueManager manager;
    public void StartDialogue()  
    {
        manager.OpenDialogue(dialogues, avatars);
       
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
    //public Sprite sprite;

}

