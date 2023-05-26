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
        if (MSTT != null)
        {
            MSTT.SetActive(true);
        }
        else
        {
            carregarProximaCena();
        }      
    }

    public void carregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena.name.ToString());
    }
}
