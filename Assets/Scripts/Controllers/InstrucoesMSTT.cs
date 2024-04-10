using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstrucoesMSTT : MonoBehaviour
{
    public UnityEvent fimInstrucoesPreSom, fimInstrucoesPosSom;
    [SerializeField] private string audioPreSom, audioPosSom;

    public void instrucoesPreSom()
    {
        // tocar instrucoes antes dos sons
        AudioController.instance.PlayDialogue(audioPreSom);
        StartCoroutine(tocarInstrucoesPreSom());
        // esperar até que o som seja concluído, e então permitir o MSTT
    }
    public void instrucoesPosSom()
    {
        AudioController.instance.PlayDialogue(audioPosSom);
        StartCoroutine(tocarInstrucoesPosSom());
    }

    private IEnumerator tocarInstrucoesPreSom()
    {
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        yield return new WaitForSeconds(1f);
        fimInstrucoesPreSom?.Invoke();
    }

    private IEnumerator tocarInstrucoesPosSom()
    {
        yield return new WaitWhile (()=> AudioController.instance.IsDialoguePlaying());
        fimInstrucoesPosSom?.Invoke();
    }
}
