using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAndDie : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayAudioAndDieCoroutine());
    }

    private IEnumerator PlayAudioAndDieCoroutine()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        while (audio.isPlaying)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }

}
