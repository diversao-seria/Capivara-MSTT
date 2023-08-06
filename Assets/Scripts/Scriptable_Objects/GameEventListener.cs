using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // ouve qualquer evento

    // cria uma referência a um scriptableObject da classe "GameEvent"
    public GameEvent Event;

    // cria um UnityEvent responsável por determinar a resposta ao evento. A criação de um UnityEvent permite que o designer escolha qual função vai ser rodada em resposta ao evento direto da interface, sem precisar mexer no código.
    public UnityEvent Response;

    // adiciona o objeto à lista de inscritos no evento
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    // remove o objeto da lista de inscritos no evento
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    // chama a função-resposta ao evento, que pode estar em qualquer outro script desse objeto e deve ser selecionada através do inspetor
    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
