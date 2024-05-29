using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public AudioClip Clip;
    public AudioSource Source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<DictionnaryElement<string, Audio>> sound;

    public void PlaySong(string name)
    {
        foreach (var item in sound)
        {
            if (item.Key == name)
            {
                item.Value.Source.PlayOneShot(item.Value.Clip);
            }
        }
    }
}
