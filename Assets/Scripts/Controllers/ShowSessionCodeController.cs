using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowSessionCodeController : MonoBehaviour
{

    [SerializeField]
    private StringVariable codigoSessao;
    [SerializeField]
    private TMP_Text texto;

    void Start()
    {
        OnSessionChange();
    }

    private void OnEnable()
    {
        BaseDeDados.NovoCodigoSessao += OnSessionChange;
    }

    private void OnDisable()
    {
        BaseDeDados.NovoCodigoSessao -= OnSessionChange;
    }
    void OnSessionChange()
    {
        texto.text = codigoSessao.Value;
    }
}
