using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResource : MonoBehaviour
{
    public PlayerController player;
    public TMPro.TextMeshProUGUI wood;

    void Update()
    {
        wood.text = $"Wood: {player.WoodResource:###,###,###,###}";
    }
}
