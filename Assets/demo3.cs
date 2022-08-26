using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo3 : MonoBehaviour
{
    public GameObject trenching;
    public GameObject trenching2;
    public GameObject tools;
    public GameObject techniques;
    public void Trench()
    {
        trenching.SetActive(true);
        techniques.SetActive(false);
        trenching2.SetActive(false);
        tools.SetActive(false);
    }

    public void Techniques()
    {
        techniques.SetActive(true);
        trenching.SetActive(false);
        trenching2.SetActive(false);
        tools.SetActive(false);
    }

    public void Trench2()
    {
        techniques.SetActive(false);
        trenching.SetActive(false);
        trenching2.SetActive(true);
        tools.SetActive(false);
    }

    public void Toool()
    {
        techniques.SetActive(false);
        trenching.SetActive(false);
        trenching2.SetActive(false);
        tools.SetActive(true);
    }

}
