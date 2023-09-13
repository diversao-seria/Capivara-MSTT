using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MSTTManager : MonoBehaviour
{
    public UnityEvent msttSucesso;
    private string s = "";
    public string resposta = "";
    public TMPro.TextMeshProUGUI textDisplay;

    public AudioClip somFino;
    public AudioClip somGrosso;
    public AudioSource fonteSom;
    public Button soundButton, oButton, iButton;

    [SerializeField] private Sprite spriteAcerto, spriteErro;
    [SerializeField] private Image spriteFeedback;

    public GameObject exit;

    [SerializeField] private bool MSTTAleatorio = true;
    [SerializeField] private List<string> sequenciasMSTT = new List<string>();

    [SerializeField] private int quantidadeTestes = 1;

    public UnityEvent ultimoMSTT;

    // evento que sinaliza o fim da sequência de sons do mstt
    public UnityEvent somParou;

    // referencia p/ as acoes do jogador (novo input system)
    PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        string s;
        if (!MSTTAleatorio)
        {
            s = sequenciasMSTT[0];
            quantidadeTestes = sequenciasMSTT.Count;
        }
        else
        {
            s = RandomString();
        }
        
        Debug.Log("Enable");
        PlayerPrefs.SetString("sequence", s);

        playerInputActions.MSTT.InputI.performed += IInput;
        playerInputActions.MSTT.InputO.performed += OInput;
    }

    void OnDisable()
    {
        playerInputActions.MSTT.InputI.performed -= IInput;
        playerInputActions.MSTT.InputO.performed -= OInput;
    }

    void Start()
    {         
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

    public void OInput(InputAction.CallbackContext context)
    {
        OButton();
    }

    public void IInput(InputAction.CallbackContext context)
    {
        IButton();
    }

    public void Confirma()
    {
        StartCoroutine(FeedbackCoroutine());
    }

    public void Cancela()
    {
        resposta = "";
    }

    public void PlaySound()
    {
        StartCoroutine(DelayDoInicio());
        playerInputActions.MSTT.Disable();
        s = PlayerPrefs.GetString("sequence");
        Debug.Log(s);
        StartCoroutine(PlaySoundCoroutine());
    }    

    IEnumerator PlaySoundCoroutine()
    {
        oButton.interactable = false;
        iButton.interactable = false;
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
        yield return new WaitForSeconds(0.682f * s.Length);
        playerInputActions.MSTT.Enable();
        oButton.interactable = true;
        iButton.interactable = true;

        somParou?.Invoke();
    }

    IEnumerator FeedbackCoroutine()
    {
        spriteFeedback.enabled = true;

        if (resposta.Equals(s))
        {
            spriteFeedback.sprite = spriteAcerto;
            yield return new WaitForSeconds(2f);
            Debug.Log("Acertou");


            if (quantidadeTestes <= 1)
            {
                msttSucesso?.Invoke();
                yield break;
            }
            else if (MSTTAleatorio)
            {
                s = RandomString();               
                quantidadeTestes -= 1;
            }
            else
            {
                sequenciasMSTT.RemoveAt(0);
                s = sequenciasMSTT[0];
                quantidadeTestes = sequenciasMSTT.Count;
            }
            PlayerPrefs.SetString("sequence", s);

        }
        else
        {
            spriteFeedback.sprite = spriteErro;
            yield return new WaitForSeconds(2f);
        }
        
        if (quantidadeTestes == 2)
        {
            ultimoMSTT?.Invoke();
        }
        spriteFeedback.enabled = false;
        Cancela();
        PlaySound();
    }

    public void CloseButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DelayDoInicio()
    {
        yield return new WaitForSeconds(1f);
    }

}
