using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolData
{
    public string id;
    public int cost;
    

    public ToolData(Tool tool)
    {
        id = tool.id;
        cost = tool.cost;  
    }
}
