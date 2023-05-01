using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClipReference pink_audio;
    [SerializeField] AudioClipReference blue_audio;
    AudioSource source;
    
    void OnEnable()
    {
        FilaDeNotas.NotePlayed += playSound;
    }
    void OnDisable()
    {
        FilaDeNotas.NotePlayed -= playSound;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void playSound(char note)
    {
        
        if (note == 'I')
        {
            source.clip = pink_audio.Value;
        }
        else if (note == 'O')
        {
            source.clip = blue_audio.Value;
        }
        source.Play(0);      
    }
}
