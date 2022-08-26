using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo3 : MonoBehaviour
{
    public GameObject trenching;
    public GameObject techniques;
    public void Trench()
    {
        trenching.SetActive(true);
        techniques.SetActive(false);
    }

    public void Techniques()
    {
        techniques.SetActive(true);
        trenching.SetActive(false);
    }
}
