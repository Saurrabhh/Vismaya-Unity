using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBackStory : MonoBehaviour
{
    public GameObject canvas;
    public Animator animator;
    public AnimationClip clip;
    public bool start;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && start)
        {
            StartCoroutine(StartPick());
            start = false;
        }
    }

    IEnumerator StartPick()
    {
        animator.SetTrigger("BookPick");
        Debug.Log("Lalala");
        yield return new WaitForSeconds((clip.length * 2)*2f);
        SceneManager.LoadScene((int)Scenes.Summary);
    }

    private void OnTriggerExit(Collider other)
    {
        start = false;
        canvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        start = true;
        canvas.SetActive(true);
    }
}
