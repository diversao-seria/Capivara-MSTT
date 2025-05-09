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
using UnityEditor.Overlays;

[CreateAssetMenu]
public class AudioController : ScriptableObject
{
    // public static AudioController instance { get; private set; }
    private EventInstance musicaEventInstance, notaMSTTEventInstance, dialogueInstance, oneShotNivelInstance;
    [SerializeField] private FMODEvents fmodEvents;
    FMOD.Studio.EVENT_CALLBACK dialogueCallback;
    public TimelineInfo timelineInfo = null;
    private GCHandle timelineHandle;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;

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
        dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvents.fala);

        // Pin the key string in memory and pass a pointer through the user data
        GCHandle stringHandle = GCHandle.Alloc(key);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        dialogueInstance.setCallback(dialogueCallback);
        dialogueInstance.start();
        dialogueInstance.release();
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

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void Setup()
    {
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        musicaEventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicaEventInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Erro no Timeline Callback: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
            timelineInfo.currentBeat = parameter.beat;

        }
        return FMOD.RESULT.OK;
    }

    private void OnDestroy() 
    {
        musicaEventInstance.release();
        timelineHandle.Free();
    }


    
}
