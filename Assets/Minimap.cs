using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform player;

    private void LateUpdate()
    {
        Vector3 new_Position = player.position;
        new_Position.y = transform.position.y;
        transform.position = new_Position;
    }
}
