using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public Avatar[] avatars;
    public Animator animator;
    [SerializeField] AnimationClip clip;
    public bool start = false;
    
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
            Debug.Log("msdcmdsc");
            StartCoroutine(StartDialogueAsync());
        }

    }
    

    IEnumerator StartDialogueAsync()
    {
        animator.SetTrigger("Buzzer");
        yield return new WaitForSeconds(clip.length);
        StartDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        start = true;
        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        start = false;
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

