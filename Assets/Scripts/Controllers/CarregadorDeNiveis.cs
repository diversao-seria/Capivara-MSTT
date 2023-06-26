using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregadorDeNiveis : MonoBehaviour
{
    public GameObject MSTT;
    public Object proximaCena;

    public void jogadorPassouNaPorta()
    {
        StartCoroutine(Espera(1)); 
    }

    public void carregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena.name.ToString());
    }

    IEnumerator Espera(int tempo)
    {
        yield return new WaitForSeconds(tempo);
        if (MSTT != null)
        {
            MSTT.SetActive(true);
        }
        else
        {
            carregarProximaCena();
        }
    }
}
