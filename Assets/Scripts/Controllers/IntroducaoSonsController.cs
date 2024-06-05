using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroducaoSonsController : MonoBehaviour
{
    [SerializeField] private Image imagemAgudo, imagemGrave;  
    [SerializeField] private Sprite imagemAgudoTocando, imagemAgudoPadrao, imagemGraveTocando, imagemGravePadrao;
    [SerializeField] private ParticleSystem particulasAgudo, particulasGrave;
    [SerializeField] private string falaInicio, falaGrave1, falaAgudo1, falaMeio, falaGrave2, falaAgudo2, falaFim;
    [SerializeField] private AudioController audioController;
    [SerializeField] private FMODEvents fmodEvents;

    void Start()
    {
        StartCoroutine(TocarSons());
    }

    IEnumerator TocarSons()
    {
        audioController.PlayDialogue(falaInicio);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaGrave1);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.tocarOneShotMSTT(fmodEvents.MSTTGrave);
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitForSeconds(1f);
        particulasGrave.Stop();
        audioController.pararOneShotMSTT();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaAgudo1);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.tocarOneShotMSTT(fmodEvents.MSTTAgudo);
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitForSeconds(1f);
        particulasAgudo.Stop();
        audioController.pararOneShotMSTT();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaMeio);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaGrave2);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.tocarOneShotMSTT(fmodEvents.MSTTGrave);
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitForSeconds(1f);
        particulasGrave.Stop();
        audioController.pararOneShotMSTT();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaAgudo2);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        audioController.tocarOneShotMSTT(fmodEvents.MSTTAgudo);
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitForSeconds(1f);
        particulasAgudo.Stop();
        audioController.pararOneShotMSTT();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        audioController.PlayDialogue(falaFim);
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("tutorial_1");
    }
}
