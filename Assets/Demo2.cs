using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Demo2 : MonoBehaviour
{
    public GameObject g;
    // Start is called before the first frame update
    public void click()
    {
        g.SetActive(false);
        Diggggg p = FindObjectOfType<Diggggg>();
        p.enabled = true;

    }
}
