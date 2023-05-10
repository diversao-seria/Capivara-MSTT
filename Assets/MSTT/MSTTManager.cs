using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MSTTManager : MonoBehaviour
{
    private string s = "";
    public string resposta = "";
    public TMPro.TextMeshProUGUI textDisplay;

    public AudioClip somFino;
    public AudioClip somGrosso;
    public AudioSource fonteSom;
    public Button soundButton;

    public GameObject exit;

    void OnEnable()
    {
        string s = RandomString();
        Debug.Log("Enable");
        PlayerPrefs.SetString("sequence", s);
        PlaySound();
    }

    
    // Update is called once per frame
    void Update()
    {
        textDisplay.text = resposta;

    }

    public string RandomString()
    {
        char[] chars = new char[2] { 'O', 'I' };
        string t = "";
        for(int i = 0; i < 4; i++)
        {
            t += chars[Random.Range(0, 2)];
        }
        return t;
    }

    public void OButton()
    {
        resposta += 'O';
    }

    public void IButton()
    {
        resposta += 'I';
    }

    public void Confirma()
    {
        if(resposta.Equals(s))
        {
            Debug.Log("Acertou");
            // this.transform.parent.gameObject.SetActive(false);
            // exit.SetActive(true);
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Errou");
        }
    }

    public void Cancela()
    {
        resposta = "";
    }

    public void PlaySound()
    {
        s = PlayerPrefs.GetString("sequence");
        Debug.Log(s);
        StartCoroutine(PlaySoundCoroutine());
    }    

    IEnumerator PlaySoundCoroutine()
    {
        for (int i = 0; i < s.Length; i++)
        {
            if(s[i] == 'I')
            {
                fonteSom.clip = somFino;
            }
            else
            {
                fonteSom.clip = somGrosso;
            }

            fonteSom.Play();
            yield return new WaitForSeconds(0.682f);
        }
    }

    public void CloseButton()
    {
        // this.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }


}
