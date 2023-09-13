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


    public void SomTerminou()
    {
        if (posicoesFinais.Count > 0)
        {
            StartCoroutine(TimerTutorial());
        } 
    }

    public void ClicouNaInterface()
    {
        StopAllCoroutines();
        //StopCoroutine(MoveCursor());
        // StopCoroutine(PiscarCursor());
        posicoesFinais.RemoveAt(0);
        if (posicoesFinais.Count > 0)
        {
            StartCoroutine(MoveCursor());
        }
        else
        {
            componenteImagem.enabled = false;
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
        Debug.Log("OOOO");
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
            posicaoIntermediaria = Vector3.Lerp(posicaoAntes, posicoesFinais[0].position, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(PiscarCursor());      
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
