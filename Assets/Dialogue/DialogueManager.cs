using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject techniquesCanvas;
    public GameObject dialogueTrigger;
    public Image avatarImage;
    public TextMeshProUGUI avatarName;
    public TextMeshProUGUI dialogueText;
    public RectTransform background;
    Collision collision;
    Dialogue[] currentDialogues;  //to store the details of current active sessions
    Avatar[] currentAvatars;
    int activeDialogue = 0;
    public static bool isActive = false;
    public void OpenDialogue(Dialogue[] dialogues, Avatar[] avatars)
    {
        currentDialogues = dialogues;
        currentAvatars = avatars;
        activeDialogue = 0;
        isActive = true;
        Debug.Log("Started" + dialogues.Length);
        DisplayMessage();
        background.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        Dialogue dialogueToDisplay = currentDialogues[activeDialogue];
        dialogueText.text = dialogueToDisplay.dialogue;
        
        Avatar avatarToDisplay = currentAvatars[dialogueToDisplay.AvatarId];
        avatarName.text = avatarToDisplay.name;
        avatarImage.sprite = avatarToDisplay.sprite;

        AnimateText();
    }

    public void Nextmessage()
    {
        activeDialogue++;
        if(activeDialogue < currentDialogues.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("convo ended");
            background.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            isActive = false;
        }
    }

    void AnimateText()
    {
        LeanTween.textAlpha(dialogueText.rectTransform, 0, 0);
        LeanTween.textAlpha(dialogueText.rectTransform, 1, 1f);
    }
    void Start()
    {
        background.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isActive==true)
        {
            Nextmessage();
        }
        else if (Input.GetKeyDown(KeyCode.N) && isActive == true)
        {
            SceneManager.LoadScene((int)Scenes.StoryofRulers);
        }
        else if (Input.GetKeyDown(KeyCode.M) && isActive == true)
        {
            techniquesCanvas.SetActive(true);
            Diggggg diggggg = FindObjectOfType<Diggggg>();
            diggggg.enabled = false;
            dialogueTrigger.SetActive(false);
        }

    }
}
