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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && start)
        {
            StartCoroutine(StartDialogueAsync());
            start = false;
        }

    }
    

    IEnumerator StartDialogueAsync()
    {
        animator.SetTrigger("Buzzer");
        Player player = FindObjectOfType<Player>();
        player.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        yield return new WaitForSeconds(clip.length * 2);
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

