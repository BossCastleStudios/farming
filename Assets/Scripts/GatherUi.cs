using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherUi : MonoBehaviour
{
    public Image image;
    public PlayerController player;

    void Update()
    {
        image.color = player.CanGatherResources && !player.IsGathering ? Color.white : Color.clear;
    }
}
