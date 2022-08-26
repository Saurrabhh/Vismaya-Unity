using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Demo : MonoBehaviour
{
    public GameObject[] gameObjects;
    int index = 0;

    // Update is called once per frame
    private void Start()
    {
        Diggggg p = FindObjectOfType<Diggggg>();
        p.enabled = false;
    }

    public void Change()
    {
        gameObjects[index].gameObject.SetActive(false);
        index = (index + 1) % gameObjects.Length;
        gameObjects[index].gameObject.SetActive(true);

    }
}
