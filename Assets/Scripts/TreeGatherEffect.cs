using UnityEngine;

public class TreeGatherEffect : MonoBehaviour, IGatherEffect
{
    public GameObject leaves;
    public GameObject trunk;
    public ParticleSystem particles;

    public void StartGathering()
    {
        particles.Play();
    }

    public void OnGather(float newPercent)
    {
        if (newPercent < 0.3)
        {
            trunk.transform.localScale = new Vector3(1, 1, 0.5f);
            trunk.transform.localPosition = new Vector3(0, 0.25f, 0);
        }
        else if (newPercent < 0.5)
        {
            trunk.transform.localScale = new Vector3(1, 1, 1);
            trunk.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else if (newPercent < 0.7)
        {
            leaves.SetActive(false);
        }
    }
}