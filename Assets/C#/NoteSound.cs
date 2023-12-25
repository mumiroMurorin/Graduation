using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        audioSource.PlayOneShot(audioClip);
    }

    void Update()
    {
        if (!audioSource.isPlaying) { Destroy(this.gameObject); }
    }
}
