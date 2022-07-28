using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    AudioSource audioSource;
    [SerializeField] AudioClip transitionAudio;
    bool makingTransition = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        { 
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        makingTransition = true;
        audioSource.Stop();
        audioSource.PlayOneShot(transitionAudio);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;
    
        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Play Animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(levelIndex);
    }
}