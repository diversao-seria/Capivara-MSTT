using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeProgresso : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float tempoPreenchimento = 2.728f;
    private float valorAlvo = 0f, velocidadePreenchimento = 0.5f;

    void OnEnable()
    {
        slider.value = 0;
    }

    void Start()
    {    
        slider.value = 0;
        MudarValor(1f);
        velocidadePreenchimento = (valorAlvo) / tempoPreenchimento;
    }

    void FixedUpdate()
    {
        if(slider.value < valorAlvo)
        {
            slider.value += velocidadePreenchimento * Time.fixedDeltaTime;
        }
        
    }

    void MudarValor(float valorNovo)
    {
        valorAlvo = slider.value + valorNovo;
    }
}
