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
    [SerializeField] private List<AudioClip> sonsFeedback;
    [SerializeField] private AudioSource fonteSom;
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
                fonteSom.clip = sonsFeedback[codigoErro];
                break;
            case 1:
                spriteFeedback.sprite = spriteErro; 
                fonteSom.clip = sonsFeedback[codigoErro];
                break;
            case 2:
                spriteFeedback.sprite = spriteErro;
                fonteSom.clip = sonsFeedback[codigoErro];
                break;
            case 3:
                spriteFeedback.sprite = spriteErro;
                fonteSom.clip = sonsFeedback[codigoErro];
                break;
            case 10:
                fonteSom.clip = sonsFeedback[4]; 
                spriteFeedback.sprite = spriteAcerto;   
                break; 
            default:
                fonteSom.clip = sonsFeedback[0]; 
                spriteFeedback.sprite = spriteErro;   
                break;           
        }
        StartCoroutine(SequenciaDeFeedback());
    }

    IEnumerator SequenciaDeFeedback()
    {      
        spriteFeedback.enabled = true;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);
        spriteFeedback.enabled = false;
        feedbackTerminou?.Invoke();
    }
}
