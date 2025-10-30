using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RodrigoTeste : MonoBehaviour
{

    [SerializeField]
    private StringVariable codigoSessao;

    public static event Action NovoCodigoSessao;
    // Start is called before the first frame update
    void Start()
    {
        codigoSessao.Value = GeraChavePrimaria();
        NovoCodigoSessao?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //essa funcao gera uma chave primaria unica
    private string GeraChavePrimaria()
    {
        //lista de possiveis caracteres (I e O foram retirados para evitar confusão com os numeros 1 e 0)
        char[] letrasNumeros = "ABCDEFGHJKLMNPQRSTUVWXYZ1234567890".ToCharArray();
        //lista de chars onde sera criada a chave primaria
        char[] chavePrimaria = new char[7];
        //instaciacao da classe Random
        var rand = new System.Random();
        for (int i = 0; i < 6; i++)
        {
            chavePrimaria[i] = letrasNumeros[rand.Next(34)]; //aleatoriamente sorteia um  letrasNumeros
        }
        //define o identificador
        chavePrimaria[6] = GeraCheckDigit(chavePrimaria);
        string chaveString = new string(chavePrimaria);
        Debug.Log(chaveString);

        return chaveString;
    }


    //essa funcao gera um digito de confirmacao para a parte aleatoria da chave, usando o algoritmo de Luhn ajustado para comportar letras
    private char GeraCheckDigit(char[] chave)
    {
        int count = 0;
        for (int i = 0; i<6; i++)
        {
            if (i%2 == 0)
            {
                count += CharParaCheckInt(chave[i]) * 2;
            }
            else
            {
                count += CharParaCheckInt(chave[i]);
            }
        }
        count = ((10 - (count % 10)) % 10) + 48;
        return Convert.ToChar(count);
    }

    // essa funcao auxiliar a GeraCheckDigit devolve um numero de 1 a 9 para os caracteres que recebe, podendo esse serem [0-9] ou [A-Z]
    private int CharParaCheckInt(char c)
    {
        int i = (int)c;
        if (i >= 48 && i <= 57)
        {
            i = i - 48;
            return i;
        }
        else if(i >= 65 && i <= 90)
        {
            i = ((i - 65) % 9)+1;
            return i;
        }
        else
        {
            throw new ArgumentException("Valores tem que ser letra maiuscula ou numero");
        }
    }

    public void OnCodigoCriado()
    {
        Debug.Log("O codigo foi criado");
    }
}
