using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MSTTFeedback : MonoBehaviour
{
    [SerializeField] private Image spriteFeedback;
    [SerializeField] private Sprite spriteAcerto, spriteErro;
    [SerializeField] private List<String> sonsFeedback;
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
            case 0:
                spriteFeedback.sprite = spriteErro;
                AudioController.instance.PlayDialogue(sonsFeedback[codigoErro]);
                break;
            case 1:
                spriteFeedback.sprite = spriteErro; 
                AudioController.instance.PlayDialogue(sonsFeedback[codigoErro]);
                break;
            case 2:
                spriteFeedback.sprite = spriteErro;
                AudioController.instance.PlayDialogue(sonsFeedback[codigoErro]);
                break;
            case 3:
                spriteFeedback.sprite = spriteErro;
                AudioController.instance.PlayDialogue(sonsFeedback[codigoErro]);
                break;
            case 4:
                spriteFeedback.sprite = spriteErro;
                AudioController.instance.PlayDialogue(sonsFeedback[codigoErro]);
                break;
            case 10:
                AudioController.instance.PlayDialogue(sonsFeedback[5]);
                spriteFeedback.sprite = spriteAcerto;   
                break; 
            default:
                AudioController.instance.PlayDialogue(sonsFeedback[0]);
                spriteFeedback.sprite = spriteErro;   
                break;           
        }
        StartCoroutine(SequenciaDeFeedback());
    }

    IEnumerator SequenciaDeFeedback()
    {      
        spriteFeedback.enabled = true;
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);
        spriteFeedback.enabled = false;
        feedbackTerminou?.Invoke();
    }
}
