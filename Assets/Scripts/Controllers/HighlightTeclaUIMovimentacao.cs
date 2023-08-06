using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HighlightTeclaUIMovimentacao : MonoBehaviour
{
    public Sprite normal, highlight;
    private Image image;
    PlayerInputActions playerInputActions;
    public string tecla;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void OnEnable()
    {
        playerInputActions.Player.Movimentacao.performed += TeclaPressionada;
        playerInputActions.Player.Movimentacao.canceled += TeclaSolta;     
    }

    void OnDisable()
    {
        playerInputActions.Player.Movimentacao.performed -= TeclaPressionada;
        playerInputActions.Player.Movimentacao.canceled -= TeclaSolta;
    }

    void Start()
    {
        image = this.GetComponent<Image>();
        image.sprite = normal;
    }

    public void TeclaPressionada(InputAction.CallbackContext context)
    {
        if ("Key:/Keyboard/" + tecla == context.control.ToString())
        {
            image.sprite = highlight;
        }       
    }

    public void TeclaSolta(InputAction.CallbackContext context)
    {
        if ("Key:/Keyboard/" + tecla == context.control.ToString())
        {
            image.sprite = normal;
        }
    }
}
