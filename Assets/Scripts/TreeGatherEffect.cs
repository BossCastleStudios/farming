using System.Collections;
using UnityEngine;

public class TreeGatherEffect : MonoBehaviour, IGatherEffect
{
    public GameObject leaves;
    public GameObject trunk;
    public ParticleSystem particles;
    public AudioSource chopping;
    public GameObject treeFall;

    private bool instantiatedTreeFall = false;

    public void StartGathering()
    {
        particles.Play();
        chopping.Play();
    }

    public void OnGather(float newPercent)
    {
        if (newPercent < 0.01)
        {
            trunk.SetActive(false);
            particles.Stop();
        }
        else if (newPercent < 0.33)
        {
            trunk.transform.localScale = new Vector3(1, 1, 0.5f);
            trunk.transform.localPosition = new Vector3(0, 0.25f, 0);
            if (!instantiatedTreeFall)
            {
                Instantiate(treeFall);
                instantiatedTreeFall = true;
            }
            particles.Stop();
        }
        else if (newPercent < 0.5)
        {
            trunk.transform.localScale = new Vector3(1, 1, 1);
            trunk.transform.localPosition = new Vector3(0, 0.5f, 0);
            leaves.SetActive(false);
        }
        else if (newPercent < 0.7)
        {
            leaves.SetActive(false);
        }
    }

    public void OnRegen(float newPercent)
    {
        if (newPercent > 0.7)
        {
            leaves.SetActive(true);
        }
        else if (newPercent > 0.5)
        {
            trunk.transform.localScale = new Vector3(1, 1, 2);
            trunk.transform.localPosition = new Vector3(0, 1f, 0);
        }
        else if (newPercent > 0.01f)
        {
            trunk.transform.localScale = new Vector3(1, 1, 1f);
            trunk.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else
        {
            trunk.SetActive(true);
        }
    }

    public void StopGathering()
    {
        particles.Stop();
        chopping.Stop();
    }
}