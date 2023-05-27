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
        movePlayer();
        gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y);
        gridPosition.UseConstant = false;   
    }

    void Update()
    {
        // Checagem de inputs (pode mudar posteriormente para encaixar checagens de diferentes valores poss�veis p/ cada c�lula)
        // Emitem o evento "moveAttempt" e checam a resposta. Se for diferente de -1 significa que a c�lula desejada � v�lida e o jogador pode andar sobre ela.
        if (Input.GetKeyDown("right") && moveAttempt?.Invoke(gridPosition.Value.x + 1, gridPosition.Value.y) != -1)
        {
            // Se passar na checagem, atualiza as coordenadas atuais do jogador, movendo-o e emitindo o evento responsável por informar que o jogador se moveu.
            gridPosition.Value = new Vector2Int(gridPosition.Value.x + 1, gridPosition.Value.y);
            movePlayer();
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("left") && moveAttempt?.Invoke(gridPosition.Value.x - 1, gridPosition.Value.y) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x - 1, gridPosition.Value.y);
            movePlayer();
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("up") && moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y + 1) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y + 1);
            movePlayer();
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("down") && moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y - 1) != -1)
        {
            gridPosition.Value = new Vector2Int(gridPosition.Value.x, gridPosition.Value.y - 1);
            movePlayer();
            playerMoved?.Invoke();
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Checagem do valor da c�lula atual do jogador. Nesse caso, se a c�lula atual tiver valor 1 o teste MSTT é iniciado.
        if (moveAttempt?.Invoke(gridPosition.Value.x, gridPosition.Value.y) == 1)
        {
            passouNaPorta?.Invoke();
            // SceneManager.LoadScene(1);
           // Debug.Log("hey");
        }
    }

    private void movePlayer()
    {
        transform.position = grid.getWorldPosition(gridPosition.Value.x, gridPosition.Value.y) + Vector3.up * playerHeight;
    }

    private void OnPlataformMoved(int gridXAntigaPlataforma, int gridYAntigaPlataforma, int gridXNovaPlataforma, int gridYNovaPlataforma)
    {
        if (gridPosition.Value.x == gridXAntigaPlataforma && gridPosition.Value.y == gridYAntigaPlataforma)
        {
            gridPosition.Value = new Vector2Int (gridXNovaPlataforma, gridYNovaPlataforma);

            movePlayer();
        }
    }
}
