using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBackStory : MonoBehaviour
{
    public GameObject canvas;
    public Animator animator;
    public AnimationClip clip;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(StartPick());
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
        canvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }
}
