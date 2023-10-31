using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrucoesController : MonoBehaviour
{
    // Scriptable Object que conta a quantidade de tentativas
    public IntReference tentativas;

    // essa vari�vel define a partir de qual tentativa as instru��es devem ser seguidas
    public int inicioDasInstrucoes = 1;

    [SerializeField] private AudioClip somInicio;
    [SerializeField] private List<AudioClip> sonsBotaoGrave, sonsBotaoAgudo, sonsFeedback;
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
        MSTTManager.CodigoErroMSTT += ReproduzirNarracaoFeedback;
    }
    void OnDisable()
    {
        GridController.NotePlayed -= ReproduzirSomBotao;
        MSTTManager.CodigoErroMSTT -= ReproduzirNarracaoFeedback;
    }

    public void InterrompeNarracao()
    {
        // interrompe as instru��es caso o jogador chegue ao final do n�vel
        fonteSom.Stop();
    }

    public void ReproduzirSomBotao(char notaEvento)
    {
        if (notaEvento == 'O')
        {
            fonteSom.clip = sonsBotaoGrave[Random.Range (0, sonsBotaoGrave.Count)];
        }
        else if (notaEvento == 'I')
        {
            fonteSom.clip = sonsBotaoAgudo[Random.Range (0, sonsBotaoAgudo.Count)];
        }
        fonteSom.Play();
    }

    public void ReproduzirNarracaoFeedback(int codigoErroMstt)
    {
        switch (codigoErroMstt)
        {
            case 1:
                fonteSom.clip = sonsFeedback[0];
                break;
            case 2:
                fonteSom.clip = sonsFeedback[1];
                break;
            case 3:
                fonteSom.clip = sonsFeedback[2];
                break;
            case 4:
                fonteSom.clip = sonsFeedback[3];
                break;
            case 10:
                fonteSom.clip = sonsFeedback[4];    
                break;            
        }
    }
}
