using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    // cria uma lista para manter todos os objetos que estão inscritos no evento
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // provoca o evento
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].OnEventRaised();
    }

    // adiciona um objeto à lista
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    // remove um objeto da lista
    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
