using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregadorDeNiveis : MonoBehaviour
{
    public GameObject MSTT;
    public string proximaCena;

    public void jogadorPassouNaPorta()
    {
        if (MSTT != null)
        {
            StartCoroutine(Espera(1));
        }
        else
        {
            carregarProximaCena();
        }
    }

    public void carregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena);
    }

    IEnumerator Espera(int tempo)
    {
        yield return new WaitForSeconds(tempo);
        MSTT.SetActive(true);
    }
}
