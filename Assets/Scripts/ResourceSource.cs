using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public ResourceType resourceType;
    public int resourceAmount;

    private int initialAmount;
    private IGatherEffect effect;

    void Awake()
    {
        initialAmount = resourceAmount;
        effect = GetComponent<IGatherEffect>();
    }

    void Update()
    {
        if (resourceAmount <= 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    public int GatherResource(int removeAmount)
    {
        int amountBeforeRemoval = resourceAmount;
        resourceAmount = Mathf.Max(0, resourceAmount - removeAmount);
        effect.OnGather(resourceAmount / (float)initialAmount);
        return amountBeforeRemoval - resourceAmount;
    }

    public void StartGathering()
    {
        effect.StartGathering();
    }
}

public enum ResourceType
{
    Wood,
    Rock
}