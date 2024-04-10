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

    void Start()
    {
        StartCoroutine(TocarSons());
    }

    IEnumerator TocarSons()
    {
        AudioController.instance.PlayDialogue(falaInicio);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaGrave1);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.tocarOneShotMSTT(FMODEvents.instance.MSTTGrave);
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitForSeconds(1f);
        particulasGrave.Stop();
        AudioController.instance.pararOneShotMSTT();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaAgudo1);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.tocarOneShotMSTT(FMODEvents.instance.MSTTAgudo);
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitForSeconds(1f);
        particulasAgudo.Stop();
        AudioController.instance.pararOneShotMSTT();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaMeio);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaGrave2);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.tocarOneShotMSTT(FMODEvents.instance.MSTTGrave);
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitForSeconds(1f);
        particulasGrave.Stop();
        AudioController.instance.pararOneShotMSTT();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaAgudo2);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.tocarOneShotMSTT(FMODEvents.instance.MSTTAgudo);
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitForSeconds(1f);
        particulasAgudo.Stop();
        AudioController.instance.pararOneShotMSTT();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        AudioController.instance.PlayDialogue(falaFim);
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("tutorial_1");
    }
}
