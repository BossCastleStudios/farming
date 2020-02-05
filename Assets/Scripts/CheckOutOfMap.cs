using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutOfMap : MonoBehaviour
{
    public float heightDetection = -100.0f;

    void Update()
    {
        if (this.transform.position.y < heightDetection)
        {
            Destroy(this.gameObject);
        }
    }
}
