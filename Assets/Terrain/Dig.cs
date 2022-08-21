using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour
{
    // Start is called before the first frame update
    public Terrain t;
    bool[,] holes = new bool[10, 10];

    private void Start()
    {
        
           
            for (var x = 0; x < 10; x++)
                for (var y = 0; y < 10; y++)
                    holes[x, y] = !(x > 2 && x < 8 && y > 2&& y < 8);
            //t.terrainData.SetHoles(0, 0, holes);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = -Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                worldPosition = hit.point;
                
                t.terrainData.SetHoles((int)worldPosition.x, (int)worldPosition.y, holes);
            }
            Debug.Log(worldPosition);
        }
        
        
    }

    
}
