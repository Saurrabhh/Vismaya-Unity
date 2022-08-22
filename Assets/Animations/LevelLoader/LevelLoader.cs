using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    
    AudioSource audioSource;
    [SerializeField] AudioClip transitionAudio;
    public Slider slider;
    public GameObject panel;
    //bool makingTransition = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void LoadLevel(Scenes levelIndex)
    {
        //makingTransition = true;
        
        
    
        StartCoroutine(LoadLevelAsync(levelIndex));
    }

    IEnumerator LoadLevelAsync(Scenes levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)levelIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            panel.SetActive(true);
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            slider.value = progress;

            if(progress >= 1.0f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        audioSource.Stop();
        audioSource.PlayOneShot(transitionAudio);
    }
}