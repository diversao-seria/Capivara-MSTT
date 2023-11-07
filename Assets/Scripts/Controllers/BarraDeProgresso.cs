using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeProgresso : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float tempoPreenchimento = 2.728f;
    private float valorAlvo = 0f, velocidadePreenchimento = 0.5f;

    public void SomParou()
    {
        
    }

    void Start()
    {
        MudarValor(100f);
        // Debug.Log(Time.fixedDeltaTime);
        velocidadePreenchimento = (valorAlvo / 100) / tempoPreenchimento;
        // Debug.Log("velocidade " + velocidadePreenchimento);
    }

    void FixedUpdate()
    {
        if(slider.value < valorAlvo)
        {
            // Debug.Log(slider.value);
            slider.value += velocidadePreenchimento * Time.fixedDeltaTime;
        }
    }

    void MudarValor(float valorNovo)
    {
        valorAlvo = slider.value + valorNovo;
    }
}
