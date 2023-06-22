using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GridController : MonoBehaviour
{
    public Grid grid;
    [SerializeField] private int gridLength = 1;
    [SerializeField] private int gridHeight = 1;
    [SerializeField] private int cellSize = 1;
    [SerializeField] private Transform gridOrigin;
    [SerializeField] private TextAsset levelText;

    private Vector2Int portaCoords = new Vector2Int();

    public Vector2Reference playerPos;

    // evento emitido quando o jogador passa pela porta aberta
    public UnityEvent passouNaPorta;

    // evento emitido ao tocar uma nota. Passa a nota tocada como parametro do tipo Char
    public static event Action<char> NotePlayed;

    private void OnEnable()
    {
        // Inscreve o GridController no evento moveAttempt (usado para fazer a comunica��o entre jogador e grid)
        PlayerMovementController.moveAttempt += returnValue;

        PlataformMovementController.PlataformMoved += OnPlataformMoved;  //Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>
    }

    private void OnDisable()
    {
        // remove o evento moveAttempt do GridController (importante p/ evitar memory leaks)
        PlayerMovementController.moveAttempt -= returnValue;

        PlataformMovementController.PlataformMoved -= OnPlataformMoved;  // Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>

    }

    // Start is called before the first frame update
    private void Awake()
    {
        GridLevel level = JsonUtility.FromJson<GridLevel>(levelText.text);

        gridLength = level.layers[0].width;
        gridHeight = level.layers[0].height;

        // Cria uma nova inst�ncia da grid
        grid = new Grid(gridLength, gridHeight, cellSize, gridOrigin.transform.position);

        // valor a ser checado na lista dos valores dos tiles vindos do tiled
        int ID = 0;

        // esses dois for loops convertem o array em uma dimensão (fornecido pelo mapa do tiled) em um array de duas dimensões (usado na grid)
        for (int i = level.layers[0].height - 1; i >= 0; i--)
        {
            for (int j = 0; j < level.layers[0].width; j++)
            {
                // converte o ID dos tiles do tiled nos números a serem usados dentro do jogo
                switch(level.layers[0].data[ID])
                {
                    case 1:
                        grid.setValue(j, i, 0);
                        break;
                    case 2:
                        grid.setValue(j, i, -1);
                        break;
                    case 3:
                        portaCoords = new Vector2Int(j, i);
                        grid.setValue(j, i, -1);
                        break;
                    case 4:
                        grid.setValue(j, i, 3);
                        break;
                    case 5:
                        grid.setValue(j, i, 4);
                        break;
                }
                ID++;
            }
        }
    }

    public Vector3 getWorldPosition(int x, int y)
    {
        // Converte coordenadas (X, Y) em coordenadas da Unity
        return grid.GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f;
    }

    private int returnValue(int x, int y)
    {
        // retorna o valor armazenado na c�lula (X, Y) da grid
        return grid.returnValue(x, y);
    }

    // Altera o valor das casa do grid ocupadas por uma plataforma, permitindo que o jogador va para uma posicao ocupada por uma plataforma mesmo se essa e normalmente inocupavel, garante tambem que depois da plataforma
    // garante tambem que depois da plataforma sair o jogador volta a nao poder ocupar essa posicao
    private void OnPlataformMoved(int gridXAntigaPlataforma, int gridYAntigaPlataforma, int gridXNovaPlataforma, int gridYNovaPlataforma)
    {
        // retira a possiblidade do jogador ocupar uma posicao quando a plataforma saiu dela, o valor 2 na grid indica uma posicao ocupavel temporariamente
        if (returnValue(gridXAntigaPlataforma, gridYAntigaPlataforma) == 2){
            grid.setValue(gridXAntigaPlataforma, gridYAntigaPlataforma, -1);
        }
        
        // da a possiblidade do jogador ocupar uma posicao quando a plataforma assume ela, o valor 2 na grid indica uma posicao ocupavel temporariamente
        if (returnValue(gridXNovaPlataforma, gridYNovaPlataforma) == -1){
            grid.setValue(gridXNovaPlataforma, gridYNovaPlataforma, 2);
        }
    }

    // Altera o valor das casa do grid ocupadas pela porta, permitindo que o jogador chegue no fim do nivel 
    public void OnDoorUnlocked()
    {
        grid.setValue(portaCoords.x, portaCoords.y, 1);
        Debug.Log("Porta aberta");
    }

    // Recebe o evento "playerMoved" e checa o valor da casa em que o jogador está p/ agir de acordo
    public void JogadorMoveu()
    {
        switch (returnValue(playerPos.Value.x, playerPos.Value.y))
        {
            case 1:
                // emite o evento avisando que o jogador passou pela porta
                passouNaPorta?.Invoke();
                break;
            case 3:
                // emite o evento de mudar as plataformas de cor rosa de lugar
                NotePlayed?.Invoke('I');
                Debug.Log("I");
                break;
            case 4:
                // emite o evento de mudar as plataformas de cor azul de lugar
                NotePlayed?.Invoke('O');
                Debug.Log("O");
                break;
        }       
    }
}

[System.Serializable]
public class GridLevel
{
    public List<Layer> layers;
}

[System.Serializable]
public class Layer
{
    public int[] data;
    public int height;
    public int width;
}
