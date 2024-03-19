using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class encerramentoController : MonoBehaviour
{
    public UnityEvent EncerrarJogo;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Salvando...");
        EncerrarJogo?.Invoke();
    }
}
