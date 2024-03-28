using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MSTTManager : MonoBehaviour
{
    public static event Action<int> CodigoErroMSTT;
    public UnityEvent msttSucesso, msttErro, msttInicio;
    private string s = "";
    public string resposta = "";
    public TMPro.TextMeshProUGUI textDisplay;

    public AudioClip somFino;
    public AudioClip somGrosso;
    public AudioSource fonteSom;
    public Button soundButton, oButton, iButton, confirmButton, deleteButton;

    [SerializeField] private Sprite spriteAcerto, spriteErro;
    [SerializeField] private Image spriteFeedback;
    [SerializeField] private GameObject AmpulhetaPanel;

    public GameObject exit;

    [SerializeField] private bool MSTTAleatorio = true, testeInstruido = false, tocandoInstrucoes = true;
    [SerializeField] private List<string> sequenciasMSTT = new List<string>();

    [SerializeField] private int quantidadeTestes = 1;

    public UnityEvent ultimoMSTT;
    public UnityEvent FimMSTTUN;

    // evento que sinaliza o fim da sequência de sons do mstt
    public UnityEvent somParou;

    // referencia p/ as acoes do jogador (novo input system)
    PlayerInputActions playerInputActions;
    private bool feedbackTerminou = false;

    private Vector2Int quantidadeSimbolosMSTT = new Vector2Int(0, 0);

    public bool temNarracao = false;
    public StringReference respostaMSTT;

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

        MSTTFeedback.feedbackTerminou += FimDoFeedback;
        playerInputActions.MSTT.InputI.performed += IInput;
        playerInputActions.MSTT.InputO.performed += OInput;
        Debug.Log(s);
    }

    void OnDisable()
    {
        MSTTFeedback.feedbackTerminou -= FimDoFeedback;
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

        if (resposta.Length > 0)
        {
            deleteButton.interactable = true;
        }
        else
        {
            deleteButton.interactable = false;
        }

    }

    public string RandomString()
    {
        char[] chars = new char[2] { 'O', 'I' };
        string t = "";
        for(int i = 0; i < 4; i++)
        {
            t += chars[UnityEngine.Random.Range(0, 2)];
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
        respostaMSTT.Value = resposta;
        StartCoroutine(FeedbackCoroutine());
    }

    public void Cancela()
    {
        resposta = "";
    }

    public void PlaySound()
    {
        msttInicio?.Invoke();
        StartCoroutine(DelayDoInicio());
    }    

    IEnumerator PlaySoundCoroutine()
    {
        
        confirmButton.interactable = false;
        oButton.interactable = false;
        iButton.interactable = false;
        
        // Se não tem narração aguarda um tempo antes de tocar o som
        if(!temNarracao)
        {
            AmpulhetaPanel.SetActive(true);
            yield return new WaitForSeconds(0.682f * 4);
            AmpulhetaPanel.SetActive(false);
        }


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

        fonteSom.Stop();
        AmpulhetaPanel.SetActive(true);
        yield return new WaitForSeconds(0.682f * /*s.Length*/ 4);
        AmpulhetaPanel.SetActive(false);

        playerInputActions.MSTT.Enable();
        
        if (!testeInstruido)
        {
            AlterarEstadosBotoes(true);
        }       

        somParou?.Invoke();
    }

    IEnumerator FeedbackCoroutine()
    {
        FimMSTTUN?.Invoke();

        // se o teste tiver instruções, emitir o evento de feedback
        if (testeInstruido)
        {
            AlterarEstadosBotoes(false);
            feedbackTerminou = true;
            int codigoErro = checarErro(resposta);
            CodigoErroMSTT?.Invoke(codigoErro);
            yield return new WaitWhile (()=> feedbackTerminou);

            if (codigoErro == 10)
            {
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
                tocandoInstrucoes = true;
                msttErro?.Invoke();               
            }
        }
        else
        {
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
 
        Cancela();
        PlaySound();
    }

    public void CloseButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator DelayDoInicio()
    {
        if (testeInstruido)
        {
            yield return new WaitWhile (()=> tocandoInstrucoes);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }        
        playerInputActions.MSTT.Disable();
        s = PlayerPrefs.GetString("sequence");
        quantidadeSimbolosMSTT = ContarSimbolosMSTT(s);
        StartCoroutine(PlaySoundCoroutine());
    }

    public void FimDoFeedback()
    {
        feedbackTerminou = false;
    }

    public void fimInstrucoesPreSom()
    {
        tocandoInstrucoes = false;
    }

    public void fimInstrucoesPosSom()
    {
        confirmButton.interactable = true;
        oButton.interactable = true;
        iButton.interactable = true;
    }

    public void AlterarEstadosBotoes(bool estado)
    {
        confirmButton.interactable = estado;
        oButton.interactable = estado;
        iButton.interactable = estado;
    }

    private int checarErro(string respostaMSTT)
    {
        bool diferencaOrdemSequencia = CompararRespostaMSTT(respostaMSTT);
        
        quantidadeSimbolosMSTT = ContarSimbolosMSTT(s);

        if (string.IsNullOrEmpty(respostaMSTT)) return 0;
        else if (respostaMSTT.Length < s.Length) return 1;
        else if (respostaMSTT.Length > s.Length) return 2;
        else if (!respostaMSTT.Equals(s) && !diferencaOrdemSequencia) return 3;
        else if (!respostaMSTT.Equals(s) && diferencaOrdemSequencia) return 4;
        else if (Truncate(respostaMSTT, 11).Equals(s)) return 10;
        else return 404;
    }

    private Vector2Int ContarSimbolosMSTT(string stringMSTT)
    {
        int quantidadeO = 0, quantidadeI = 0;
   
        foreach (char c in stringMSTT)
        {
            if (c.Equals('O'))
            {
                quantidadeO++;
            }
            else if (c.Equals('I'))
            {
                quantidadeI++;
            }
        }

        return new Vector2Int(quantidadeO, quantidadeI);
    }

    private bool CompararRespostaMSTT(string respostaMSTT)
    {
        Vector2Int quantidadeResposta = ContarSimbolosMSTT(respostaMSTT);
        Debug.Log("MSTT: " + quantidadeSimbolosMSTT + " | Resposta: " + quantidadeResposta);

        if (quantidadeResposta == quantidadeSimbolosMSTT)
        {
            return true;
        }
        else
        {
            return false;
        }
    }   

    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
    }

    public void ActivateDeleteButton()
    {
        if(resposta.Length > 0)
        {
            deleteButton.interactable = true;
        }
        else
        {
            deleteButton.interactable = false;
        }
    }

    public void DeleteLastChar()
    {
        if (resposta.Length > 0)
        {
            resposta = resposta.Remove(resposta.Length - 1);
        }
    }

}
