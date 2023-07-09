using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrucoesNivel3 : MonoBehaviour
{
    private bool portaFechada = true;
    public Vector2Reference posicaoJogador;
    private int tentativasPorta = 1;

    // Lógica: se o jogador chegar no tile ao lado da porta, sair e voltar pro mesmo tile, a instrução será tocada. Caso o jogador pegue a chave, a instrução será cancelada. Essa parte será hard coded, uma vez que
    // só deve acontecer nessa fase.

    // funcao chamada quando o jogador se movimentar
    public void JogadorMoveu()
    {
        // checa se a posicao do jogador é igual à posicao da porta
        if (posicaoJogador.Value.x == 1 && posicaoJogador.Value.y == 0 && portaFechada)
        {
            // checa se a porta está fechada e se e a primeira vez do jogador nessa posicao
            if (tentativasPorta % 2 == 0)
            {
                GetComponent<AudioSource>().Play();
            }
            tentativasPorta++;
        }
    }

    public void PortaAbriu()
    {
        portaFechada = false;
        GetComponent<AudioSource>().Stop();
    }
}
