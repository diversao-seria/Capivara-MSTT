using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrucoesController : MonoBehaviour
{
    // Scriptable Object que conta a quantidade de tentativas
    public IntReference tentativas;

    // essa variável define a partir de qual tentativa as instruções devem ser seguidas
    public int inicioDasInstrucoes = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tentativa" + tentativas.Value.ToString());
        // Garante que a instrução será reproduzida a cada três tentativas
        if (tentativas.Value == inicioDasInstrucoes || (tentativas.Value % 3) == 0)
        {
            this.GetComponent<AudioSource>().Play();
        }    
    }

    public void InterrompeNarracao()
    {
        // interrompe as instruções caso o jogador chegue ao final do nível
        this.GetComponent<AudioSource>().Stop();
    }
}
