using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class CursorTutorialController : MonoBehaviour
{
    [SerializeField] private List<Transform> posicoesFinais = new List<Transform>();
    [SerializeField] private Sprite spriteClique, spritePadrao;
    [SerializeField] private float tempoDeAnimacao = 0.5f, esperaCursor = 2f;
    [SerializeField] private AnimationCurveReference animCurve;
    [SerializeField] private Image componenteImagem;
    [SerializeField] private GameObject cursorTutorialPrefab;
    private bool spriteAtualClique = false;
    private bool timerRodando = false;
    private float tempoAtual;
    private int posicaoAtual = 0;
    private bool cursorPiscando = false; 

    public void SomTerminou()
    {
        if (posicoesFinais.Count > 0)
        {
            tempoAtual = esperaCursor;
            timerRodando = true;
            posicaoAtual = 0;
        } 
    }

    public void ClicouNaInterface()
    {
        StopAllCoroutines();
        
        if (!timerRodando)
        {
            if (posicaoAtual < posicoesFinais.Count)
            {
                StartCoroutine(ControladorAnimacaoCursor());
            }
            else
            {
                componenteImagem.enabled = false;
            }
        }
        else
        {
            posicaoAtual++;
            tempoAtual = esperaCursor;
        }    
    }

    public void ErrouMSTT()
    {
        StopAllCoroutines(); 

        componenteImagem.enabled = false;
        cursorPiscando = false;

        tempoAtual = esperaCursor;  
        
        timerRodando = true;
    }

    public void MSTTAcerto()
    {
        componenteImagem.enabled = false;
        cursorPiscando = false;
        timerRodando = false;
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
                Debug.Log(posicaoAtual);
                componenteImagem.enabled = true;
                timerRodando = false;
                cursorPiscando = true;
                StartCoroutine(ControladorAnimacaoCursor());
            }
        }         
    }

    IEnumerator ControladorAnimacaoCursor()
    {
        yield return MoveCursor();

        posicaoAtual++;

        yield return new WaitForSeconds(0.3f);

        yield return PiscarCursor();
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
    }

    IEnumerator PiscarCursor()
    {
        while (cursorPiscando)
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
