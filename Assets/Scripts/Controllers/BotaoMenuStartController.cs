using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotaoMenuStartController : MonoBehaviour
{
    [SerializeField] private  GameObject mensagemAudio;
    [SerializeField] private Button botaoStart;
    [SerializeField] private AudioController audioController;

    void Start()
    {
        audioController.PararMusica(true);
    }
    public void IniciarTutorial()
    {
        audioController.criarDialogueCallback();
        StartCoroutine(MensagemAudio());
        botaoStart.interactable = false;
    }

    IEnumerator MensagemAudio()
    {
        mensagemAudio.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("HQ");
    }
}
