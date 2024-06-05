using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class CursorTutorialController : MonoBehaviour
{
    [SerializeField] private List<Transform> posicoesFinais = new List<Transform>();
    [SerializeField] private Sprite spriteClique, spritePadrao;
    [SerializeField] private float tempoDeAnimacao = 0.5f;
    [SerializeField] private AnimationCurveReference animCurve;
    [SerializeField] private Image componenteImagem;
    private bool spriteAtualClique = false;
    private int posicaoAtual = 0;
    private Coroutine moveCursor, piscarCursor;

    void OnEnable()
    {
        MSTTFeedback.feedbackTerminou += Reset;
    }
    void OnDisable()
    {
        MSTTFeedback.feedbackTerminou -= Reset;
    } 

    public void SomTerminou()
    {
        componenteImagem.enabled = true;
        moveCursor = StartCoroutine(MoveCursor());       
    }

    public void ClicouNaInterface()
    {
       if (posicaoAtual < posicoesFinais.Count - 1)
       {
            if (moveCursor != null)
            {
                StopCoroutine(moveCursor);
                moveCursor = null;
            }
            if (piscarCursor != null)
            {
                StopCoroutine(piscarCursor);
                piscarCursor = null;
            }           
            
            posicaoAtual++;
            moveCursor = StartCoroutine(MoveCursor());
       }
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

        yield return new WaitForSeconds(0.3f); 
        piscarCursor = StartCoroutine(PiscarCursor());      
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
    
    public void Reset()
    {
        StopCoroutine(piscarCursor);

        componenteImagem.enabled = false;
        posicaoAtual = 0;
        Debug.Log(posicaoAtual);
    }
}
