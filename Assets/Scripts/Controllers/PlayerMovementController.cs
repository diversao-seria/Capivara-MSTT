using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class PlayerMovementController : MonoBehaviour
{
    // curva de animação do jogador
    public AnimationCurveReference animCurve;

    // altura do jogador em relação ao solo
    [SerializeField] private float playerHeight = 0.5f;

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

    // referencia p/ as acoes do jogador (novo input system)
    PlayerInputActions playerInputActions;

    // checa se o jogador está se movendo
    private bool movendo = false;

    // Lista dos inputs
    private List<Vector2Int> listaInputsMovimentacao = new List<Vector2Int>();

    // Token p/ cancelar a função async "MovedorPlayer"
    CancellationTokenSource tks = null;

    // ultimo input do jogador
    Vector2Int ultimoInput;

    // angulo de rotação da capivara
    float angulo = 0f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void OnEnable()
    {
        PlataformMovementController.PlataformMoved += OnPlataformMoved;  // Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>
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
        gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y);
        transform.position = grid.getWorldPosition(gridPosition.Value.x, gridPosition.Value.y) + Vector3.up * playerHeight;
        gridPosition.UseConstant = false;
        ultimoInput = new Vector2Int(1, 0); // Inicializa a variaval e define o valor padrão para a orientação definida no prefab (! ALTERAÇÕES NO PREFAB PODEM GERAR PROBLMEAS NA SINCRONIA DA ROTAÇÃO !)
    }

    public void Reiniciar(InputAction.CallbackContext context)
    {
        gameOver?.Invoke();
    }

    public void FimDeJogo()
    {
        // Cancela a função async MovedorPlayer
        tks.Cancel();

        // Se livra do token
        tks.Dispose();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MoverJogador(InputAction.CallbackContext context)
    {
        Vector2Int vetorMovimentacao = Vector2Int.RoundToInt(context.ReadValue<Vector2>());

        // Calcula a rotação do jogador baseado no input inserido e no input anterior e rotaciona a capivara de acordo
        if (!movendo)
        {
            angulo = Vector2.SignedAngle(ultimoInput, vetorMovimentacao) * -1;
            transform.Rotate(0, angulo, 0, Space.Self);
            ultimoInput = vetorMovimentacao;
        }

        // soma de todos os inputs armazenados no buffer
        Vector2Int somaListaInputs = listaInputsMovimentacao.Aggregate(new Vector2Int(0, 0), (s, v) => s + v) + vetorMovimentacao;

        // recebe o valor correspondente à casa do próximo input
        int? casa = moveAttempt?.Invoke(gridPosition.Value.x + somaListaInputs.x, gridPosition.Value.y + somaListaInputs.y);

        if (movendo)
        {
            if (casa != -1 && casa != 2)
            {
                AdicionarInput(vetorMovimentacao);
            }
        }
        else if (casa != -1)
        {
            AdicionarInput(vetorMovimentacao);
            movePlayer((new Vector2Int(gridPosition.Value.x + vetorMovimentacao.x, gridPosition.Value.y + vetorMovimentacao.y)), 0.5f, false);
        }               
    }
    
    private void OnPlataformMoved(int gridXAntigaPlataforma, int gridYAntigaPlataforma, int gridXNovaPlataforma, int gridYNovaPlataforma)
    {        
        if (gridPosition.Value.x == gridXAntigaPlataforma && gridPosition.Value.y == gridYAntigaPlataforma)
        {
            gridPosition.Value = new Vector2Int (gridXNovaPlataforma, gridYNovaPlataforma);

            listaInputsMovimentacao.Clear();
            movendo = true;
            movePlayer(new Vector2Int(gridXNovaPlataforma, gridYNovaPlataforma), 1f, true);
        }
    }

    private async void movePlayer(Vector2Int novasCoordenadas, float tempoDeAnimacao, bool plataforma)
    {
        if (!plataforma)
        {
            // Calcula a rotação do jogador baseado no input inserido e no input anterior e rotaciona a capivara de acordo
            angulo = Vector2.SignedAngle(ultimoInput, listaInputsMovimentacao[0]) * -1;
            transform.Rotate(0, angulo, 0, Space.Self);
            ultimoInput = listaInputsMovimentacao[0];
        }

        tks = new CancellationTokenSource();
        var token = tks.Token;

        Vector3 posicaoAntes = transform.position;
        Vector3 posicaoDepois = grid.getWorldPosition(novasCoordenadas.x, novasCoordenadas.y) + Vector3.up * playerHeight;
        if (plataforma) { playerInputActions.Player.Disable(); }

        await MovedorPlayer(posicaoAntes, posicaoDepois, tempoDeAnimacao, plataforma, token);

        if (!plataforma)
        {
            FinalizarMovimentacao();
        }
        else
        {
            listaInputsMovimentacao.Clear();
            playerInputActions.Player.Enable();
        }
    }

    public void PassouNaPorta()
    {
        playerInputActions.Player.Disable();
    }

    private async Task MovedorPlayer(Vector3 posicaoAntes, Vector3 posicaoDepois, float tempoDeAnimacao, bool plataforma, CancellationToken token)
    {
        Debug.Log("oi");
        movendo = true;
        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoIntermediaria = new Vector3(0, 0, 0);
        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            t = animCurve.Value.Evaluate(t);
            posicaoIntermediaria = Vector3.Lerp(posicaoAntes, posicaoDepois, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            await Task.Yield();

            if (token.IsCancellationRequested)
            {
                return;
            }
        }
        transform.position = posicaoDepois; 
    }

    private void AdicionarInput(Vector2Int input)
    {
        if (listaInputsMovimentacao.Count < 2)
        {
            listaInputsMovimentacao.Add(input);
        }
    }

    private void FinalizarMovimentacao()
    {
        gridPosition.Value = gridPosition.Value + listaInputsMovimentacao[0];
        listaInputsMovimentacao.RemoveAt(0);
        playerMoved?.Invoke();
        if (listaInputsMovimentacao.Count > 0) 
        {
            movePlayer(gridPosition.Value + listaInputsMovimentacao[0], 0.5f, false);
            return;
        }
        movendo = false;
    }

    private int valorNaoNuloV2(Vector2Int vetor)
    {
        if (vetor.x != 0)
        {
            return vetor.x;
        }
        return vetor.y;
    }
}
