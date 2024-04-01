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
    [SerializeField] private AudioClip somInicio;
    [SerializeField] private List<AudioClip> sonsBotaoGrave, sonsBotaoAgudo;
    private AudioSource fonteSom;
    
    
    // Start is called before the first frame update
    void Start()
    {
        fonteSom = this.GetComponent<AudioSource>();
        fonteSom.clip = somInicio;
        Debug.Log("tentativa" + tentativas.Value.ToString());
        // Garante que a instru��o ser� reproduzida a cada tr�s tentativas
        if (tentativas.Value == inicioDasInstrucoes || (tentativas.Value % 3) == 0)
        {
            fonteSom.Play();
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
        fonteSom.Stop();
    }

    public void ReproduzirSomBotao(char notaEvento)
    {
        if (tocarInstrucoesBotoes)
        {
            if (notaEvento == 'O')
            {
                // fonteSom.clip = sonsBotaoGrave[Random.Range (0, sonsBotaoGrave.Count)];
                AudioController.instance.tocarOneShot(FMODEvents.instance.feedbackBotaoFaseGrave);
            }
            else if (notaEvento == 'I')
            {
                // fonteSom.clip = sonsBotaoAgudo[Random.Range (0, sonsBotaoAgudo.Count)];
                AudioController.instance.tocarOneShot(FMODEvents.instance.feedbackBotaoFaseAgudo);
            }
            // fonteSom.Play();
        }
        
    }
}
