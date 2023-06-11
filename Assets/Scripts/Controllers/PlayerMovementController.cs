using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
    // altura do jogador em relação ao solo
    [SerializeField] private float playerHeight = 3;

    // evento emitido ao mover o jogador
    public UnityEvent playerMoved;

    // evento emitido ao passar pela porta aberta
    public UnityEvent passouNaPorta;

    // coordenadas do jogador em relacao ao grid
    public Vector2Reference gridPosition;

    // refer�ncia � grid (depend�ncia a ser investigada depois)
    [SerializeField] private GridController grid;

    // evento emitido ao mover o jogador, passando as coordenadas novas no grid como parametro <posicaoXnova, posicaoYnova>
    // public static event Action<int, int> PlayerMoved;

    // evento que retorna o valor armazenado em uma c�lula (X, Y) da grid (usado para checar se a casa p/ onde o jogador est� tentando se mover � v�lida ou n�o. Recebe como par�metros dois inteiros (coordenadas X e Y) e retorna um inteiro (valor guardado na c�lula (X, Y) da grid.))
    public static Func<int, int, int> moveAttempt;

    Coroutine movedorPlayer;

    void OnEnable()
    {
        PlataformMovementController.PlataformMoved += OnPlataformMoved;  //Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>
    }
    
    void OnDisable()
    {
        PlataformMovementController.PlataformMoved -= OnPlataformMoved;
    }
    
    void Start()
    {
        gridPosition.UseConstant = true;
        movePlayer(gridPosition.Value.x, gridPosition.Value.y, 0.1f);
        gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y);
        gridPosition.UseConstant = false;   
    }

    void Update()
    {
        int gridXantiga = gridPosition.Value.x;
        int gridYantiga = gridPosition.Value.y;
        // Checagem de inputs (pode mudar posteriormente para encaixar checagens de diferentes valores poss�veis p/ cada c�lula)
        // Emitem o evento "moveAttempt" e checam a resposta. Se for diferente de -1 significa que a c�lula desejada � v�lida e o jogador pode andar sobre ela.
        if (Input.GetKeyDown("right") && moveAttempt?.Invoke(gridPosition.Value.x + 1, gridPosition.Value.y) != -1)
        {
            // Se passar na checagem, atualiza as coordenadas atuais do jogador, movendo-o e emitindo o evento responsável por informar que o jogador se moveu.
            gridPosition.Value = new Vector2Int(gridPosition.Value.x + 1, gridPosition.Value.y);
            movePlayer(gridXantiga, gridYantiga, 0.5f);
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("left") && moveAttempt?.Invoke(gridPosition.Value.x - 1, gridPosition.Value.y) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x - 1, gridPosition.Value.y);
            movePlayer(gridXantiga, gridYantiga, 0.5f);
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("up") && moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y + 1) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y + 1);
            movePlayer(gridXantiga, gridYantiga, 0.5f);
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("down") && moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y - 1) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y - 1);
            movePlayer(gridXantiga, gridYantiga, 0.5f);
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Checagem do valor da c�lula atual do jogador. Nesse caso, se a c�lula atual tiver valor 1 o teste MSTT é iniciado.
        if (moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y) == 1)
        {
            // emite o evento avisando que o jogador passou pela porta
            passouNaPorta?.Invoke();
            // desabilita o movement controller para que o jogador não possa mais se movimentar
            this.enabled = false;
        }
    }

    private void movePlayer(int gridXantiga, int gridYantiga, float tempoDeAnimacao)
    {
        Vector3 posicaoAntes = grid.getWorldPosition(gridXantiga, gridYantiga) + Vector3.up * playerHeight;
        Vector3 posicaoDepois = grid.getWorldPosition(gridPosition.Value.x, gridPosition.Value.y) + Vector3.up * playerHeight;
        movedorPlayer = StartCoroutine(MovedorPlayer(posicaoAntes, posicaoDepois, tempoDeAnimacao));
    }

    private void OnPlataformMoved(int gridXAntigaPlataforma, int gridYAntigaPlataforma, int gridXNovaPlataforma, int gridYNovaPlataforma)
    {
        if (gridPosition.Value.x == gridXAntigaPlataforma && gridPosition.Value.y == gridYAntigaPlataforma)
        {
            gridPosition.Value = new Vector2Int (gridXNovaPlataforma, gridYNovaPlataforma);

            movePlayer(gridXAntigaPlataforma, gridYAntigaPlataforma, 1f);
        }
    }

    IEnumerator MovedorPlayer(Vector3 posicaoAntes, Vector3 posicaoDepois, float tempoDeAnimacao)
    {
        float t = 0;
        float tempoPassado = 0;
        Vector3 posicaoIntermediaria = new Vector3 (0,0,0);
        while (tempoPassado <= tempoDeAnimacao)
        {
            t = tempoPassado / tempoDeAnimacao;
            posicaoIntermediaria = Vector3.Lerp (posicaoAntes, posicaoDepois, t);
            transform.position = posicaoIntermediaria;
            tempoPassado += Time.deltaTime;
            yield return null;
        }
        transform.position = posicaoDepois;
    }
}
