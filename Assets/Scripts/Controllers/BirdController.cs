using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BirdController : MonoBehaviour
{
    [SerializeField] private Vector2Int gridPosition = new Vector2Int();
    [SerializeField] private GridController grid;
    [SerializeField] private Vector2Reference playerPosition;
    [SerializeField] private float birdHeight = 2;
    public UnityEvent birdCollected;

    // Start is called before the first frame update
    private void Start()
    {
        transform.position = grid.getWorldPosition(gridPosition.x, gridPosition.y) + Vector3.up * birdHeight;
    }

    public void OnPlayerMoved() // Verifica se o jogador esta na mesma posicao q o passaro
    {
        if (playerPosition.Value == gridPosition)
        {
            onCollected();
        }
    }

    private void onCollected()  // Envia o sinal de que o passaro foi coletado e desativa o Objeto
    {
        birdCollected?.Invoke();
        Debug.Log("I've been collected!");
        this.gameObject.SetActive(false);
    }
}
