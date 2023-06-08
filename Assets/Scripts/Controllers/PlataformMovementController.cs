using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlataformMovementController : MonoBehaviour
{
    // referencia a grid (dependencia a ser investigada depois)
    [SerializeField] private GridController grid;

    // aloca a nota que vai gerar a mudanca no movimento
    [SerializeField] private char notaMexe = 'I';

    // coordenadas que a plataforma pode assumir em relacao ao grid 
    // A formatacao que TEM QUE SER SEGUIDA e (eliminando os +): posicaoInicialX+,+posicaoInicialY+ +posicaoDepoisDoMovimento1X+,+posicaoDepoisDoMovimento1Y+ +posicaoDepoisDoMovimento2X+,+posicaoDepoisDoMovimento2Y+#  
    //O '#' e necessario e indica o fim da string
    [SerializeField] private string posicoesDaPlataforma = "1,1 1,9";

    //quantidade de diferentes posicoes que a plataforma pode ocupar
    [SerializeField] private int quantidadePosicoes = 2;

    // mostra qual das posicoes possiveis a plataforma esta ocupando
    [SerializeField] private int posicaoAtual = 0;

    // declara as variaveis gridX e gridY, a funcao Star ja chama a pegaNovaPosicaoPlataforma seguido de um movePlataforma entao esses valores sao sobre escritos antes no primeiro frame
    [SerializeField] private int gridX = 0;
    [SerializeField] private int gridY = 0;

    // declara o valor que define em Vector3 (nao na grid tendo em vista que por enquanto ela e 2D) a altura plataforma
    [SerializeField] private float altura = 0;


    // evento emitido ao mover a plataforma, passando as coordenadas antigas e novas no grid como parametro <posicaoXantiga, posicaoYantiga, posicaoXnova, posicaoYnova>
    public static event Action<int, int, int, int> PlataformMoved;

    void OnEnable()
    {
        FilaDeNotas.NotePlayed += OnNotePlayed;
    }
    void OnDisable()
    {
        FilaDeNotas.NotePlayed -= OnNotePlayed;
    }

    // Start is called before the first frame update
    void Start()
    {

        // define as coordenadas da nova posicao
        pegaNovaPosicaoPlataforma();

        // atualiza a posicao com as coordenadas novas
        movePlataforma();

        // Emite o sinal que a plataforma se moveu, mas disendo que ela foi de onde ela estava para onde ela esta
        // isso corrige um Bug que inpedia o jogador de ir para uma plataforma que estava fora da area normalmente andavel no primeiro movimento
        PlataformMoved?.Invoke(gridX, gridY, gridX, gridY);



         //Se inscreve ao evento (action) NotePlayed, essa action passa como parametro um char indicando o tipo de nota tocada
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNotePlayed (char note)
    {
        if (note == notaMexe)
        {

            int gridXantiga = gridX;
            int gridYantiga = gridY;
            
            //passa a posicaoAtual para a proxima
            posicaoAtual = (posicaoAtual+1) % quantidadePosicoes;

            //define as coordenadas da nova posicao
            pegaNovaPosicaoPlataforma();

            //atualiza a posicao com as coordenadas novas
            movePlataforma();

            //Emite o sinal que a plataforma se moveu, passanda as coordenadas do grid que ela agora ocupa
            PlataformMoved?.Invoke(gridXantiga, gridYantiga, gridX, gridY);
        }
    }

    //Se baseia nos valores: posicaoAtual e posicoesDaPlataforma para alterar o gridX e gridY para os valores a serem assumidos apos a nota gatilho ser tocada
    private void pegaNovaPosicaoPlataforma()
    {
        int i = 0;
        int j = 0;
        string tempString = string.Empty;
        //percore a string posicoesDaPlataforma contando o numero de espacos, tendo em vista que a informacao da posicao n da plataforma esta apos o n espaco
        //ao sair da condicao o j esta posicionado sobre o primeiro digito da gridX da posicao nova da plataforma.
        while (i != posicaoAtual)
        {
            j += 1;
            if (posicoesDaPlataforma[j-1] == ' ')
            {
                i += 1;
            }
        }

        //percore a string posicoesDaPlataforma comessando no primeiro valor do gridX da nova posicao da plataforma ate a ',' salvando os numeros percorridos ele coloca no tempString o valor do gridX em string
        while (posicoesDaPlataforma[j] != ',')
        {
            tempString += posicoesDaPlataforma[j];

            j += 1;
        }

        //da ao gridX o conteudo da tempString so que como int
        gridX = int.Parse(tempString);

        //limpamos o conteudo da tempString
        tempString = "";

        //deslisamos o j da ','
        j += 1;

        //percore a string posicoesDaPlataforma comessando no primeiro valor do gridY da nova posicao da plataforma ate o ' ' ou '#' salvando os numeros percorridos ele coloca no tempString o valor do gridY em string
        while (posicoesDaPlataforma[j] != ' ' && posicoesDaPlataforma[j] != '#')
        {
            tempString += posicoesDaPlataforma[j];
            j += 1;
        }

        //da ao gridY o conteudo da tempString so que como int
        gridY = int.Parse(tempString);

        //limpamos o conteudo da tempString
        tempString = "";
    }

    private void movePlataforma() 
    {
        transform.position = grid.getWorldPosition(gridX, gridY) + new Vector3 (0 , altura, 0);
    }
}
