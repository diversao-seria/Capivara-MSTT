using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrucoesController : MonoBehaviour
{
    // Scriptable Object que conta a quantidade de tentativas
    public IntReference tentativas;

    // essa vari�vel define a partir de qual tentativa as instru��es devem ser seguidas
    public int inicioDasInstrucoes = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tentativa" + tentativas.Value.ToString());
        // Garante que a instru��o ser� reproduzida a cada tr�s tentativas
        if (tentativas.Value == inicioDasInstrucoes || (tentativas.Value % 3) == 0)
        {
            this.GetComponent<AudioSource>().Play();
        }    
    }

    public void InterrompeNarracao()
    {
        // interrompe as instru��es caso o jogador chegue ao final do n�vel
        this.GetComponent<AudioSource>().Stop();
    }
}
