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
    [SerializeField] private FMODEvents fmodEvents;
    
    [SerializeField] private bool tocarInstrucoesBotoes = true;

    [SerializeField] private string instrucao;
    [SerializeField] private AudioController audioController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tentativa" + tentativas.Value.ToString());
        if (tentativas.Value == inicioDasInstrucoes || (tentativas.Value % 3) == 0)
        {
            audioController.PlayDialogue(instrucao);
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
        audioController.StopDialogue();
    }

    public void ReproduzirSomBotao(char notaEvento)
    {
        if (tocarInstrucoesBotoes && !audioController.OneShotTocando())
        {
            // para a explicação do nivel caso o jogador pise em um dos botoes
            InterrompeNarracao();

            if (notaEvento == 'O')
            {
                audioController.trackableOneShot(fmodEvents.feedbackBotaoFaseGrave);
            }
            else if (notaEvento == 'I')
            {
                audioController.trackableOneShot(fmodEvents.feedbackBotaoFaseAgudo);
            }
        }
        
    }
}
