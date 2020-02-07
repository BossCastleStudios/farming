using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public bool CanGather
    {
        get
        {
            if (resourceAmount > 0)
            {
                return true;
            }
            return false;
        }
    }

    public Collider[] colliders;
    public ResourceType resourceType;
    public int resourceAmount;
    public float regenDelay = 4.0f;
    public int regenRate = 6;

    private int initialAmount;
    private IGatherEffect effect;
    private bool isGathering;
    private bool isRegening;

    void Awake()
    {
        initialAmount = Random.Range(resourceAmount - 30, resourceAmount + 20);
        resourceAmount = initialAmount;
        regenDelay = Random.Range(regenDelay - 3.0f, regenDelay + 2.0f);
        regenRate = Random.Range(regenRate - 2, regenRate + 4);
        effect = GetComponent<IGatherEffect>();
    }

    void Update()
    {
        if (resourceAmount <= 0 && !isRegening)
        {
            isRegening = true;
            StartCoroutine(RegenCoroutine());
        }
    }

    private IEnumerator RegenCoroutine()
    {
        yield return new WaitForSeconds(regenDelay);
        while (resourceAmount < initialAmount)
        {
            float percentage = resourceAmount / (float) initialAmount;
            if (percentage > 0.5f)
            {
                foreach (var c in colliders)
                {
                    c.enabled = true;
                }
            }
            effect.OnRegen(percentage);

            if (isGathering)
            {
                isRegening = false;
                break;
            }

            resourceAmount = Mathf.Min(initialAmount, resourceAmount + regenRate);
            yield return new WaitForSeconds(1);
        }

        resourceAmount = initialAmount;
        isRegening = false;
    }

    public int GatherResource(int removeAmount, int maxCapacity)
    {
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        int amountBeforeRemoval = resourceAmount;
        int amountToRemove = Mathf.Min(removeAmount, maxCapacity);
        resourceAmount = Mathf.Max(0, resourceAmount - amountToRemove);
        effect.OnGather(resourceAmount / (float)initialAmount);
        if (resourceAmount <= 0)
        {
            isGathering = false;
        }
        return amountBeforeRemoval - resourceAmount;
    }

    public void StartGathering()
    {
        effect.StartGathering();
        this.isGathering = true;
    }
}

public enum ResourceType
{
    Wood,
    Rock
}