using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public AudioSource _audioSource;

    public void PlaySound()
    {
        _audioSource.Play();
    }
}
