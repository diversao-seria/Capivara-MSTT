using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MSTTAudioController : MonoBehaviour
{
    private EventInstance notaEventInstance;
    public static MSTTAudioController instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Mais de um Audio Controller na cena.");
        }
        instance = this;
    }

    void OnEnable()
    {
        GridController.NotePlayed += ouvirNotaMSTT;
    }
    void OnDisable()
    {
        GridController.NotePlayed -= ouvirNotaMSTT;
    }

    public void tocarOneShot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }

    public void ouvirNotaMSTT(char nota)
    {
        if (nota == 'O')
        {
            tocarOneShot(FMODEvents.instance.MSTTGrave);
        }
        else if (nota == 'I')
        {
            tocarOneShot(FMODEvents.instance.MSTTAgudo);
        }
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
