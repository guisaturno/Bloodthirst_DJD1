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
    
    /// <summary>
    /// Accepts an audioclip and plays sound
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        foreach (AudioSource sound in audioSources)
        {
            // Checks if playing
            if (!sound.isPlaying)
            {
                sound.clip = clip;
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
        obj.Play();

        audioSources.Add(obj);
    }
}
