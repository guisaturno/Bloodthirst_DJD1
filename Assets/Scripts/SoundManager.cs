using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    List<AudioSource> audioSources;

    // Instances
    public static SoundManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        // Instance List
        audioSources = new List<AudioSource>();

        // Classic singleton
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        GetComponentsInChildren<AudioSource>(true, audioSources);
    }

    // Is called by it's static method to do required function
    public void _PlaySound(AudioClip clip, float vol = 1.0f, float pitch = 1.0f)
    {
        foreach (AudioSource sound in audioSources)
        {
            // Checks if playing
            if (!sound.isPlaying)
            {
                sound.clip = clip;
                sound.volume = vol;
                sound.pitch = pitch;
                sound.Play();
                return;
            }
        }

        // Creates new object/sound
        GameObject nGo = new GameObject();
        nGo.transform.parent = transform;
        nGo.name = "Sound Effect";
        AudioSource obj = nGo.AddComponent<AudioSource>(); // Adds new object to list
        obj.clip = clip;
        obj.volume = vol;
        obj.pitch = pitch;
        obj.Play();

        audioSources.Add(obj);
    }

    /// <summary>
    /// Accepts an audioclip, it's volume, pitch and plays sound
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="vol"></param>
    /// <param name="pitch"></param>
    public static void PlaySound(AudioClip clip, float vol = 1.0f, float pitch = 1.0f)
    {
        if (instance == null) return;
        instance._PlaySound(clip, vol, pitch);
    }
}
