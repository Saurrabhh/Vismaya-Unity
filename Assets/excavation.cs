using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class excavation : MonoBehaviour
{
    public GameObject[] slides;
    static int slideIndex = 0;

    public void ChangeSlide()
    {
        slides[slideIndex].SetActive(false);
        slideIndex = (slideIndex + 1) % slides.Length;
        slides[slideIndex].SetActive(true);
    }

}
