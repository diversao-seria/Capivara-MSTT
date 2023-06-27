using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    // curva de animacao do jogador
    public AnimationCurveReference animCurve;

    //corotina para mover a camera
    Coroutine movedorCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void preMSTTMove()
    {
        Vector3 posicaoAntes = transform.position;
        Vector3 posicaoDepois = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3 (0, 0.9f, -1);
        movedorCamera = StartCoroutine(MovedorCamera(posicaoAntes, posicaoDepois, 1f));
    }

    IEnumerator MovedorCamera(Vector3 posicaoAntes, Vector3 posicaoDepois, float tempoDeAnimacao)
    {
        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoIntermediaria = new Vector3 (0,0,0);
        Quaternion rotacaoIntermediaria = new Quaternion(0, 0, 0, 0);
        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            t = animCurve.Value.Evaluate(t);
            posicaoIntermediaria = Vector3.Lerp (posicaoAntes, posicaoDepois, t);
            transform.position = posicaoIntermediaria;
            rotacaoIntermediaria = Quaternion.Lerp(new Quaternion(0.9f, 0, 0, 1), new Quaternion(0, 0, 0, 1), t);
            transform.rotation = rotacaoIntermediaria;
            tempoPassado += Time.deltaTime;
            
            Debug.Log(transform.rotation);
            yield return null;
        }
        transform.position = posicaoDepois;
    }
}
