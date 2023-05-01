using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class script_porta : MonoBehaviour
{
    public IntReference passaros;                 //Variavel publica com o tipo de arquivo "Passaros_obects" (referente ao scriptableobject)
    public UnityEvent doorUnlocked;

    private void Start()
    {
        passaros.UseConstant = true;
        passaros.Value = passaros.Value; // definindo o valor do Scriptable Object como o valor constante a ser indicado no inspetor
        passaros.UseConstant = false;
    }

    public void OnBirdCollected() //Funcao que vai ser chamada quando o evento de coletar passaros acontecer
    {        
        if (passaros.Value != 0) //Caso ainda tenha algum passaro (ou seja, diferente de 0)
        {                      
            passaros.Value -= 1;            //Retire um passaro
        }
        
        if (passaros.Value == 0)
        {
            doorUnlocked?.Invoke();  //Envia um sinal dizendo q todos os passaros foram coletados e agora a porta deve estar destrancada
        }
    }
}
