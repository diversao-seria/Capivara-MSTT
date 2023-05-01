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

        PlataformMovementController.PlataformMoved -= OnPlataformMoved;  //Se inscreve ao evento (action) PlataformMoved, essa action passa as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>


    }
    // Start is called before the first frame update
    private void Awake()
    {
        // Cria uma nova inst�ncia da grid
        grid = new Grid(gridLength, gridHeight, cellSize, gridOrigin.transform.position);

        // Define valores para cada uma das c�lulas da grid
        grid.setValue(0, 9, -1);
        grid.setValue(0, 8, -1);
        grid.setValue(0, 7, -1);
        grid.setValue(0, 6, -1);
        grid.setValue(0, 5, -1);

        grid.setValue(1, 1, -1);
        grid.setValue(1, 2, -1);
        grid.setValue(1, 3, -1);

        grid.setValue(2, 1, -1);
        grid.setValue(2, 4, -1);
        grid.setValue(2, 5, -1);
        grid.setValue(2, 6, -1);
        grid.setValue(2, 7, -1);
        grid.setValue(2, 9, -1);

        grid.setValue(3, 1, -1);
        grid.setValue(3, 7, -1);
        grid.setValue(3, 9, -1);

        grid.setValue(4, 1, -1);
        grid.setValue(4, 7, -1);
        grid.setValue(4, 9, -1);

        grid.setValue(5, 1, -1);
        grid.setValue(5, 7, -1);
        grid.setValue(5, 9, -1);

        grid.setValue(6, 1, -1);
        grid.setValue(6, 7, -1);
        grid.setValue(6, 9, -1);

        grid.setValue(7, 1, -1);
        grid.setValue(7, 7, -1);
        grid.setValue(7, 9, -1);

        grid.setValue(8, 1, -1);
        grid.setValue(8, 7, -1);

        grid.setValue(9, 1, -1);
        grid.setValue(9, 4, -1);
        grid.setValue(9, 5, -1);
        grid.setValue(9, 6, -1);
        grid.setValue(9, 7, -1);
        grid.setValue(9, 8, -1);
        grid.setValue(9, 9, -1);

        grid.setValue(10, 2, -1);
        grid.setValue(10, 3, -1);

        grid.setValue(11, 2, -1);
        grid.setValue(11, 3, -1);
        grid.setValue(11, 5, -1);
        grid.setValue(11, 6, -1);
        grid.setValue(11, 7, -1);
        grid.setValue(11, 8, -1);

        grid.setValue(12, 2, -1);
        grid.setValue(12, 3, -1);
        grid.setValue(12, 5, -1);
        grid.setValue(12, 8, -1);

        grid.setValue(13, 2, -1);
        grid.setValue(13, 3, -1);
        grid.setValue(13, 5, -1);
        grid.setValue(13, 8, -1);

        grid.setValue(14, 2, -1);
        grid.setValue(14, 3, -1);
        grid.setValue(14, 5, -1);
        grid.setValue(14, 8, -1);

        grid.setValue(14, 9, -1);

        grid.setValue(15, 2, -1);
        grid.setValue(15, 3, -1);
        grid.setValue(15, 5, -1);
        grid.setValue(15, 8, -1);
        grid.setValue(15, 9, -1);

        grid.setValue(16, 2, -1);
        grid.setValue(16, 3, -1);

        grid.setValue(17, 2, -1);
        grid.setValue(17, 3, -1);
        grid.setValue(17, 4, -1);
        grid.setValue(17, 5, -1);
        grid.setValue(17, 6, -1);
        grid.setValue(17, 7, -1);
        grid.setValue(17, 8, -1);

        grid.setValue(18, 2, -1);
        grid.setValue(18, 3, -1);
        grid.setValue(18, 4, -1);
        grid.setValue(18, 5, -1);
        grid.setValue(18, 6, -1);
        grid.setValue(18, 7, -1);
        grid.setValue(18, 8, -1);
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
        grid.setValue(14, 9, 1);
        Debug.Log("Porta aberta");
    }
}
