using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroducaoSonsController : MonoBehaviour
{
    public AudioSource fonteSom;
    [SerializeField] private Image imagemAgudo, imagemGrave;
    [SerializeField] private Sprite imagemAgudoTocando, imagemAgudoPadrao, imagemGraveTocando, imagemGravePadrao;
    [SerializeField] private ParticleSystem particulasAgudo, particulasGrave;
    [SerializeField] private AudioClip falaInicio, falaGrave1, falaAgudo1, falaMeio, falaGrave2, falaAgudo2, falaFim, somAgudo, somGrave;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TocarSons());
    }

    IEnumerator TocarSons()
    {
        fonteSom.clip = falaInicio;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaGrave1;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = somGrave;
        fonteSom.Play();
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        particulasGrave.Stop();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaAgudo1;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = somAgudo;
        fonteSom.Play();
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        particulasAgudo.Stop();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaMeio;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaGrave2;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = somGrave;
        fonteSom.Play();
        imagemGrave.sprite = imagemGraveTocando;
        particulasGrave.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        particulasGrave.Stop();
        imagemGrave.sprite = imagemGravePadrao;
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaAgudo2;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = somAgudo;
        fonteSom.Play();
        imagemAgudo.sprite = imagemAgudoTocando;
        particulasAgudo.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        particulasAgudo.Stop();
        imagemAgudo.sprite = imagemAgudoPadrao;
        yield return new WaitForSeconds(0.5f);

        fonteSom.clip = falaFim;
        fonteSom.Play();
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("tutorial_1");
    }
}
