using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    void FixedUpdate()
    {
        transform.position = player.transform.position;
    }
}
