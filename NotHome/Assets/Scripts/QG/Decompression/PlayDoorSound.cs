using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDoorSound : MonoBehaviour
{
    public AudioSource _audioSourceEnter;
    public AudioSource _audioSourceExit;

    public void PlayDoorEnter()
    {
        _audioSourceEnter.Play();
    }

    public void PlayDoorExit()
    {
        _audioSourceExit.Play();
    }
}
