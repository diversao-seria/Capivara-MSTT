using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstrucoesMSTT : MonoBehaviour
{
    private AudioSource fonteSom;
    [SerializeField] private AudioClip audioPreSom, audioPosSom;
    public UnityEvent fimInstrucoesPreSom, fimInstrucoesPosSom;

    void Start()
    {
        fonteSom = this.GetComponent<AudioSource>();
    }

    public void instrucoesPreSom()
    {
        // tocar instrucoes antes dos sons
        fonteSom.clip = audioPreSom;
        fonteSom.Play();
        StartCoroutine(tocarInstrucoesPreSom());
        // esperar até que o som seja concluído, e então permitir o MSTT
    }
    public void instrucoesPosSom()
    {
        // tocar instrucoes depois da sequencia de sons
        fonteSom.clip = audioPosSom;
        fonteSom.Play();
        StartCoroutine(tocarInstrucoesPosSom());
    }

    private IEnumerator tocarInstrucoesPreSom()
    {
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        yield return new WaitForSeconds(1f);
        fimInstrucoesPreSom?.Invoke();
    }

    private IEnumerator tocarInstrucoesPosSom()
    {
        yield return new WaitWhile (()=> fonteSom.isPlaying);
        fimInstrucoesPosSom?.Invoke();
    }
}
