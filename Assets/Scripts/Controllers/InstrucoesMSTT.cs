using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstrucoesMSTT : MonoBehaviour
{
    public UnityEvent fimInstrucoesPreSom, fimInstrucoesPosSom;
    [SerializeField] private string audioPreSom, audioPosSom;
    [SerializeField] private AudioController audioController;

    public void instrucoesPreSom()
    {
        // tocar instrucoes antes dos sons
        audioController.PlayDialogue(audioPreSom);
        StartCoroutine(tocarInstrucoesPreSom());
        // esperar até que o som seja concluído, e então permitir o MSTT
    }
    public void instrucoesPosSom()
    {
        audioController.PlayDialogue(audioPosSom);
        StartCoroutine(tocarInstrucoesPosSom());
    }

    private IEnumerator tocarInstrucoesPreSom()
    {
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        yield return new WaitForSeconds(1f);
        fimInstrucoesPreSom?.Invoke();
    }

    private IEnumerator tocarInstrucoesPosSom()
    {
        yield return new WaitWhile (()=> audioController.IsDialoguePlaying());
        fimInstrucoesPosSom?.Invoke();
    }
}
