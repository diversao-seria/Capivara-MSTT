using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrucoesController : MonoBehaviour
{
    // Scriptable Object que conta a quantidade de tentativas
    public IntReference tentativas;

    // essa vari�vel define a partir de qual tentativa as instru��es devem ser seguidas
    public int inicioDasInstrucoes = 1;
    
    [SerializeField] private bool tocarInstrucoesBotoes = true;

    [SerializeField] private string instrucao;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tentativa" + tentativas.Value.ToString());
        if (tentativas.Value == inicioDasInstrucoes || (tentativas.Value % 3) == 0)
        {
            AudioController.instance.PlayDialogue(instrucao);
        }    
    }

    void OnEnable()
    {
        GridController.NotePlayed += ReproduzirSomBotao;
        
    }
    void OnDisable()
    {
        GridController.NotePlayed -= ReproduzirSomBotao;
        
    }

    public void InterrompeNarracao()
    {
        // interrompe as instru��es caso o jogador chegue ao final do n�vel
        AudioController.instance.StopDialogue();
    }

    public void ReproduzirSomBotao(char notaEvento)
    {
        if (tocarInstrucoesBotoes && !AudioController.instance.OneShotNivelTocando())
        {
            // para a explicação do nivel caso o jogador pise em um dos botoes
            InterrompeNarracao();

            if (notaEvento == 'O')
            {
                AudioController.instance.tocarOneShotNivel(FMODEvents.instance.feedbackBotaoFaseGrave);
            }
            else if (notaEvento == 'I')
            {
                AudioController.instance.tocarOneShotNivel(FMODEvents.instance.feedbackBotaoFaseAgudo);
            }
        }
        
    }
}
