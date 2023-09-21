using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class CursorTutorialController : MonoBehaviour
{
    [SerializeField] private List<Transform> posicoesFinais = new List<Transform>();
    [SerializeField] private Sprite spriteClique, spritePadrao;
    [SerializeField] private float tempoDeAnimacao = 0.5f, esperaCursor = 5f;
    [SerializeField] private AnimationCurveReference animCurve;
    [SerializeField] private Image componenteImagem;
    private bool spriteAtualClique = false;
    private bool timerRodando = true;
    private float tempoAtual;
    private int posicaoAtual = 0;

    public void SomTerminou()
    {

        // teste de commit no git do lab

        if (posicoesFinais.Count > 0)
        {
            // StartCoroutine(TimerTutorial());
            Debug.Log("Sicke :P");
        } 
    }

    public void ClicouNaInterface()
    {
        StopAllCoroutines();
        
        if (!timerRodando)
        {
            if (posicaoAtual < posicoesFinais.Count)
            {
                StartCoroutine(MoveCursor());
            }
            else
            {
                componenteImagem.enabled = false;
            }
        }
        else
        {
            // decidir se devo continuar ou interromper o tutorial após o clique
            posicaoAtual++;
            tempoAtual = esperaCursor;
        }    
    }

    public void ErrouMSTT()
    {
        componenteImagem.enabled = false;
        StopAllCoroutines();
        posicaoAtual = 0;
        tempoAtual = esperaCursor;
        timerRodando = true;
        Debug.Log(posicaoAtual);
    }


    void Start()
    {
        tempoAtual = esperaCursor;
    }
    void Update()
    {
        if (timerRodando)
        {         
            if (tempoAtual > 0)
            {
                tempoAtual -= Time.deltaTime;
            }
            else
            {
                componenteImagem.enabled = true;
                timerRodando = false;
                StartCoroutine(MoveCursor());
            }
        }         
    }

    // TODO: TROCAR A CORROTINA POR UM TIMER PRA EVITAR INTERRUPÇÕES E MANTER O TUTORIAL ATIVO MESMO SE O JOGADOR CLICAR EM ALGUM BOTÃO ANTES DO TÉRMINO DO TIMER
    IEnumerator TimerTutorial()
    {
        yield return new WaitForSeconds(esperaCursor);
        componenteImagem.enabled = true;
        StartCoroutine(MoveCursor());
    }

    IEnumerator MoveCursor()
    {     
        spriteAtualClique = false;
        componenteImagem.sprite = spritePadrao;

        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoAntes = transform.position;
        Vector3 posicaoIntermediaria = new Vector3(0, 0, 0);
        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            t = animCurve.Value.Evaluate(t);
            posicaoIntermediaria = Vector3.Lerp(posicaoAntes, posicoesFinais[posicaoAtual].position, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        posicaoAtual++;
        
        yield return new WaitForSeconds(0.3f);
        

        while (true)
        {           
            if (spriteAtualClique)
            {
                componenteImagem.sprite = spritePadrao;
            }
            else
            {
                componenteImagem.sprite = spriteClique;
            }
            spriteAtualClique = !spriteAtualClique;
            yield return new WaitForSeconds(1f);
        }

        // StartCoroutine(PiscarCursor());      
    }

    IEnumerator PiscarCursor()
    {
        while (true)
        {           
            if (spriteAtualClique)
            {
                componenteImagem.sprite = spritePadrao;
            }
            else
            {
                componenteImagem.sprite = spriteClique;
            }
            spriteAtualClique = !spriteAtualClique;
            yield return new WaitForSeconds(1f);
        }
    }
}
