using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class fluteController : MonoBehaviour
{
    // este script s� deve ser utilizado uma vez ao longo do jogo. Por este motivo, n�o � necess�rio fazer a integra��o com os unityevents.
    // ser� respons�vel por ativar a fila de notas, dando in�cio � trilha da flauta
    [SerializeField] private GridController grid;
    [SerializeField] private Vector2Int coordenadasGrid;
    [Header("Mudanca de Parametro")]
    [SerializeField] private string nomeParametro;
    [SerializeField] private float valorParametro;
    [SerializeField] private AudioController audioController;
    [SerializeField] private AnimationCurveReference animCurve;
    [SerializeField] private Vector3 posicaoCamera;
    [SerializeField] private float distanciaCamera = 2f;
    [SerializeField] private GameObject canvas;
    private Animator animator, canvasAnimator;
    private Camera cam;
    public Vector2Reference coordenadasJogador;
    public UnityEvent instrumentoColetado, fimColetaInstrumento;
    private Vector3 posicaoInicial;

    void Start()
    {
        // definir posi��o inicial com base na vari�vel coordenadasGrid, que pode ser editada no inspetor.
        transform.position = grid.getWorldPosition(coordenadasGrid.x, coordenadasGrid.y);
        animator = GetComponent<Animator>();
        cam = Camera.main;
        posicaoInicial = transform.position;
        canvasAnimator = canvas.GetComponent<Animator>();
    }

    public void jogadorMoveu()
    {
        // Semp�re que o jogador se move, checa se as coordenadas atuais do jogador s�o iguais �s coordenadas atuais da flauta. Se for true, ativa a hud e dest�i o objeto flauta.
        if (coordenadasJogador.Value == coordenadasGrid)
        {
            // fade out na musica

            // toca efeito sonoro de coleta

            // toca animacao de coleta
            animator.SetTrigger("Coleta");
            canvasAnimator.SetTrigger("Coleta");
            Vector3 posicaoFinal = new Vector3();
            posicaoFinal = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, distanciaCamera));
            instrumentoColetado?.Invoke();
            StartCoroutine(move(posicaoFinal));
        }
    }

    IEnumerator move(Vector3 posicaoDepois, float tempoDeAnimacao = 1f)
    {
        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoIntermediaria = new Vector3(0, 0, 0);

        Vector3 posicaoAntes = transform.position;

        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            t = animCurve.Value.Evaluate(t);
            posicaoIntermediaria = Vector3.Lerp(posicaoAntes, posicaoDepois, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            yield return null;
        }      
    }

    public void fimColeta()
    {
        canvasAnimator.SetTrigger("OnScreen");
        // toca efeito sonoro do instrumento em si
    }

    public void fimVisualizacao()
    {
        canvasAnimator.SetTrigger("Saida");
        posicaoInicial.y = 0.5f;
        StartCoroutine(move(posicaoInicial));
    }

    public void fimAnimacao()
    {
        // adiciona objeto na musica
        audioController.DefinirParametrosMusica(nomeParametro, valorParametro);

        // fade in na musica
        //TODO

        fimColetaInstrumento?.Invoke();
        Destroy(this.gameObject);
    }
}
