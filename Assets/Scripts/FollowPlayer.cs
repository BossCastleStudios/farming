using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }
}
