using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using FMODUnity;

public class MSTTFeedback : MonoBehaviour
{
    [SerializeField] private Image spriteFeedback;
    [SerializeField] private Sprite spriteAcerto, spriteErro;
    [SerializeField] private List<String> sonsFeedback;
    [SerializeField] private AudioController audioController;
    [SerializeField] private FMODEvents fmodEvents;
    private EventReference efeito;
    private string fala;
    public static event Action feedbackTerminou;

    void OnEnable()
    {
        MSTTManager.CodigoErroMSTT += IniciarFeedback;

    }
    void OnDisable()
    {
        MSTTManager.CodigoErroMSTT -= IniciarFeedback;

    }

    private void IniciarFeedback(int codigoErro)
    {
        switch (codigoErro)
        {
            case int n when (n >= 0 && n <=4):
                spriteFeedback.sprite = spriteErro;
                fala = sonsFeedback[codigoErro];
                efeito = fmodEvents.MSTTErro;
                break;
            case 10:
                fala = sonsFeedback[5];
                efeito = fmodEvents.MSTTAcerto;
                spriteFeedback.sprite = spriteAcerto;   
                break; 
            default:
                fala = sonsFeedback[0];
                efeito = fmodEvents.MSTTErro;
                spriteFeedback.sprite = spriteErro; 
                break;           
        }
        StartCoroutine(SequenciaDeFeedback());
    }

    IEnumerator SequenciaDeFeedback()
    {   
        spriteFeedback.enabled = true;
        audioController.trackableOneShot(efeito);
        yield return new WaitWhile (()=> audioController.OneShotTocando());
        audioController.PlayDialogue(fala);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);
        spriteFeedback.enabled = false;
        feedbackTerminou?.Invoke();
    }
}
