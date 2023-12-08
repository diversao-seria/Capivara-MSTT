using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BotaoMenuStartController : MonoBehaviour
{
    [SerializeField] private  GameObject mensagemAudio;
    [SerializeField] private Button botaoStart;
    public void IniciarTutorial()
    {
        StartCoroutine(MensagemAudio());
        botaoStart.interactable = false;
    }

    IEnumerator MensagemAudio()
    {
        mensagemAudio.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("associacao - sons");
    }
}
