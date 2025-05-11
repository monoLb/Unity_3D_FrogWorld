using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get;private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
   
    }
    public void PlaySound(AudioClip clip, Vector3 pos, float volume)
    {
        if(clip==null)
            return;
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }
}
