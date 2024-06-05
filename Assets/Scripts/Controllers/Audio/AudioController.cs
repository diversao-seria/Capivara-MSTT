using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Dynamic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

[CreateAssetMenu]
public class AudioController : ScriptableObject
{
    // public static AudioController instance { get; private set; }
    private EventInstance musicaEventInstance, notaMSTTEventInstance, dialogueInstance, oneShotNivelInstance;
    [SerializeField] private FMODEvents fmodEvents;
    FMOD.Studio.EVENT_CALLBACK dialogueCallback;
    
    /*
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
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }*/

    /*void Start()
    {
        dialogueCallback = new FMOD.Studio.EVENT_CALLBACK(DialogueEventCallback);
    }*/
    public void criarDialogueCallback()
    {
        dialogueCallback = new FMOD.Studio.EVENT_CALLBACK(DialogueEventCallback);
        // testeFunc += 1;
    }

    public void PlayMusic(EventReference musicaEventReference)
    {
        musicaEventInstance = CreateInstance(musicaEventReference);
        musicaEventInstance.start();
    }

    public void PausaMusica(bool pausa)
    {
        musicaEventInstance.setPaused(pausa);
    }

    public void PararMusica (bool fadeOut)
    {
        if (fadeOut)
        {
            musicaEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            musicaEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void DefinirParametrosMusica(string nomeParametro, float valorParametro)
    {
        musicaEventInstance.setParameterByName(nomeParametro, valorParametro);
    }

    public void tocarOneShot(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }

    public void trackableOneShot(EventReference eventReference)
    {   
        oneShotNivelInstance = CreateInstance(eventReference);
        oneShotNivelInstance.start();
        oneShotNivelInstance.release();
    }

    public bool OneShotTocando()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        oneShotNivelInstance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }


    public void tocarOneShotMSTT(EventReference eventReference)
    {
        notaMSTTEventInstance = CreateInstance(eventReference);
        notaMSTTEventInstance.start();
        notaMSTTEventInstance.release();
    }

    public void pararOneShotMSTT()
    {
        notaMSTTEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    // inicia o audio da narracao (funcao utilizada para as instrucoes e narracoes, com excecao dos botoes agudo/grave das fases)
    public void PlayDialogue(string key)
    {
        // Debug.Log(testeFunc);
        dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvents.fala);

        // Pin the key string in memory and pass a pointer through the user data
        GCHandle stringHandle = GCHandle.Alloc(key);
        // GCHandle stringHandle = GCHandle.Alloc("Assets/Instrucoes/Mundo/01_Grave");
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        dialogueInstance.setCallback(dialogueCallback);
        dialogueInstance.start();
        // dialogueInstance.release();
    }

    // interrompe o dialogo
    public void StopDialogue()
    {
        dialogueInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // checa se o dialogo está tocando
    public bool IsDialoguePlaying()
    {
        FMOD.Studio.PLAYBACK_STATE state;   
	    dialogueInstance.getPlaybackState(out state);
	    return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    // Retirado diretamente do exemplo da documentacao do FMOD. De acordo com o site, o codigo pode ser utilizado em projetos pessoais e comerciais.
    static FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        // Get the string object
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        String key = stringHandle.Target as String;

        switch (type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
            {
                FMOD.MODE soundMode = FMOD.MODE.CREATESTREAM;
                var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                if (key.Contains("."))
                {
                    FMOD.Sound dialogueSound;
                    var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);
                    Debug.Log(soundResult);
                    if (soundResult == FMOD.RESULT.OK)
                    {
                        parameter.sound = dialogueSound.handle;
                        parameter.subsoundIndex = -1;
                        Marshal.StructureToPtr(parameter, parameterPtr, false);
                    }
                }
                else
                {
                    FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                    var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
                    if (keyResult != FMOD.RESULT.OK)
                    {
                        break;
                    }
                    FMOD.Sound dialogueSound;
                    var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
                    if (soundResult == FMOD.RESULT.OK)
                    {
                        parameter.sound = dialogueSound.handle;
                        parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                        Marshal.StructureToPtr(parameter, parameterPtr, false);
                    }
                }
                break;
            }
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
            {
                var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                var sound = new FMOD.Sound(parameter.sound);
                sound.release();

                break;
            }
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
            {
                // Now the event has been destroyed, unpin the string memory so it can be garbage collected
                stringHandle.Free();

                break;
            }
        }
        return FMOD.RESULT.OK;
    }

    /*
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
    } */

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    
}
