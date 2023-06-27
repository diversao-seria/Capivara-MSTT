using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    // curva de animação do jogador
    public AnimationCurveReference animCurve;

    // altura do jogador em relação ao solo
    [SerializeField] private float playerHeight = 3;

    // evento emitido ao mover o jogador
    public UnityEvent playerMoved;

    // evento emitido ao reiniciar o jogo utilizando a tecla "r"
    public UnityEvent gameOver;

    // coordenadas do jogador em relacao ao grid
    public Vector2Reference gridPosition;

    // refer�ncia � grid (depend�ncia a ser investigada depois)
    [SerializeField] private GridController grid;

    // evento que retorna o valor armazenado em uma c�lula (X, Y) da grid (usado para checar se a casa p/ onde o jogador est� tentando se mover � v�lida ou n�o. Recebe como par�metros dois inteiros (coordenadas X e Y) e retorna um inteiro (valor guardado na c�lula (X, Y) da grid.))
    public static Func<int, int, int> moveAttempt;

    Coroutine movedorPlayer;

    // referencia p/ as acoes do jogador (novo input system)
    PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void OnEnable()
    {
        PlataformMovementController.PlataformMoved += OnPlataformMoved;  //Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>
        playerInputActions.Player.Reiniciar.performed += Reiniciar;
        playerInputActions.Player.Movimentacao.performed += MoverJogador;
    }
    
    void OnDisable()
    {
        PlataformMovementController.PlataformMoved -= OnPlataformMoved;
        playerInputActions.Player.Reiniciar.performed -= Reiniciar;
        playerInputActions.Player.Movimentacao.performed -= MoverJogador;
    }

    void Start()
    {
        gridPosition.UseConstant = true;
        movePlayer(gridPosition.Value.x, gridPosition.Value.y, 0.1f, true);
        gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y);
        gridPosition.UseConstant = false;   
    }

    public void Reiniciar(InputAction.CallbackContext context)
    {
        gameOver?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MoverJogador(InputAction.CallbackContext context)
    {
        Vector2Int vetorMovimentacao = Vector2Int.RoundToInt(context.ReadValue<Vector2>());
        if (moveAttempt?.Invoke(gridPosition.Value.x + vetorMovimentacao.x, gridPosition.Value.y + vetorMovimentacao.y) != -1)
        {
            int gridXantiga = gridPosition.Value.x;
            int gridYantiga = gridPosition.Value.y;

            gridPosition.Value = gridPosition.Value + vetorMovimentacao;
            movePlayer(gridXantiga, gridYantiga, .5f, false);
            
        }
        
    }
    
    private void OnPlataformMoved(int gridXAntigaPlataforma, int gridYAntigaPlataforma, int gridXNovaPlataforma, int gridYNovaPlataforma)
    {
        if (gridPosition.Value.x == gridXAntigaPlataforma && gridPosition.Value.y == gridYAntigaPlataforma)
        {
            gridPosition.Value = new Vector2Int (gridXNovaPlataforma, gridYNovaPlataforma);

            movePlayer(gridXAntigaPlataforma, gridYAntigaPlataforma, 1f, true);
        }
    }

    private void movePlayer(int gridXantiga, int gridYantiga, float tempoDeAnimacao, bool plataforma)
    {
        Vector3 posicaoAntes = grid.getWorldPosition(gridXantiga, gridYantiga) + Vector3.up * playerHeight;
        Vector3 posicaoDepois = grid.getWorldPosition(gridPosition.Value.x, gridPosition.Value.y) + Vector3.up * playerHeight;
        movedorPlayer = StartCoroutine(MovedorPlayer(posicaoAntes, posicaoDepois, tempoDeAnimacao, plataforma));
    }

    public void PassouNaPorta()
    {
        playerInputActions.Player.Disable();
    }


    IEnumerator MovedorPlayer(Vector3 posicaoAntes, Vector3 posicaoDepois, float tempoDeAnimacao, bool plataforma)
    {
        playerInputActions.Player.Disable();
        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoIntermediaria = new Vector3 (0,0,0);
        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            t = animCurve.Value.Evaluate(t);
            posicaoIntermediaria = Vector3.Lerp (posicaoAntes, posicaoDepois, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        transform.position = posicaoDepois;
        if (!plataforma) { playerMoved?.Invoke(); }
        playerInputActions.Player.Enable();
    }
}
