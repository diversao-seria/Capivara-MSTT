using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // ouve qualquer evento

    // cria uma refer�ncia a um scriptableObject da classe "GameEvent"
    public GameEvent Event;

    // cria um UnityEvent respons�vel por determinar a resposta ao evento. A cria��o de um UnityEvent permite que o designer escolha qual fun��o vai ser rodada em resposta ao evento direto da interface, sem precisar mexer no c�digo.
    public UnityEvent Response;

    // adiciona o objeto � lista de inscritos no evento
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    // remove o objeto da lista de inscritos no evento
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    // chama a fun��o-resposta ao evento, que pode estar em qualquer outro script desse objeto e deve ser selecionada atrav�s do inspetor
    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
