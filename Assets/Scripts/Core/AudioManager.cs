using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get;private set; }
    
    [SerializeField]
    private AudioClip falseSound, trueSound,destroySound;
    [SerializeField]
    private AudioSource audioSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.falseSound:
                audioSource.PlayOneShot(falseSound);
                break;
            case SoundType.trueSound:
                audioSource.PlayOneShot(trueSound);
                break;
            case SoundType.destroySound:
                audioSource.PlayOneShot(destroySound);
                break;
                
        }
    }
}

public enum SoundType
{
    falseSound, trueSound,destroySound
}
