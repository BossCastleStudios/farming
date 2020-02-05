using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public ResourceType resourceType;
    public int resourceAmount;
}

public enum ResourceType
{
    Wood,
    Rock
}