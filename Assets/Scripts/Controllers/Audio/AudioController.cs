using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Dynamic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class AudioController : MonoBehaviour
{
    public static AudioController instance { get; private set; }

    private EventInstance musicaEventInstance, notaMSTTEventInstance;
    [SerializeField] AudioClipReference pink_audio;
    [SerializeField] AudioClipReference blue_audio;
    AudioSource source;
    
    void OnEnable()
    {
        GridController.NotePlayed += ouvirNotaMSTT;
    }
    void OnDisable()
    {
        GridController.NotePlayed -= ouvirNotaMSTT;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Mais de um Audio Controller na cena.");
        }
        instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        PlayMusic(FMODEvents.instance.musica);
    }

    /*public void playSound(char note)
    {
        
        if (note == 'I')
        {
            source.clip = blue_audio.Value;
        }
        else if (note == 'O')
        {
            source.clip = pink_audio.Value;
        }
        source.Play(0);      
    }*/

    public void PlayMusic(EventReference musicaEventReference)
    {
        musicaEventInstance = CreateInstance(musicaEventReference);
        musicaEventInstance.start();
    }

    public void PausaMusica(bool pausa)
    {
        musicaEventInstance.setPaused(pausa);
    }

    public void DefinirParametrosMusica(string nomeParametro, float valorParametro)
    {
        musicaEventInstance.setParameterByName(nomeParametro, valorParametro);
    }

    public void tocarOneShot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }

    public void tocarOneShotMSTT(EventReference eventReference)
    {
        notaMSTTEventInstance = CreateInstance(eventReference);
        notaMSTTEventInstance.start();
    }

    public void pararOneShotMSTT()
    {
        notaMSTTEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void liberarOneShotMSTT()
    {
        notaMSTTEventInstance.release();
    }

    public void ouvirNotaMSTT(char nota)
    {
        if (nota == 'O')
        {
            tocarOneShot(FMODEvents.instance.MSTTGrave);
            DefinirParametrosMusica("Ambiente", 0f);
        }
        else if (nota == 'I')
        {
            tocarOneShot(FMODEvents.instance.MSTTAgudo);
            DefinirParametrosMusica("Ambiente", 1f);
        }
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    
}
