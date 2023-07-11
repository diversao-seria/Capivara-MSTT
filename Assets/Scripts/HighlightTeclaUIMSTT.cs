using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HighlightTeclaUIMSTT : MonoBehaviour
{
    public Sprite normal, highlight;
    private Image image;
    PlayerInputActions playerInputActions;
    public string tecla;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.MSTT.Enable();
    }

    void OnEnable()
    {
        playerInputActions.MSTT.InputI.performed += TeclaPressionada;
        playerInputActions.MSTT.InputO.performed += TeclaPressionada;
        playerInputActions.MSTT.InputO.canceled += TeclaSolta;
        playerInputActions.MSTT.InputI.canceled += TeclaSolta;
    }

    void OnDisable()
    {
        playerInputActions.MSTT.InputI.performed -= TeclaPressionada;
        playerInputActions.MSTT.InputO.performed -= TeclaPressionada;
        playerInputActions.MSTT.InputO.canceled -= TeclaSolta;
        playerInputActions.MSTT.InputI.canceled -= TeclaSolta;
    }

    void Start()
    {
        image = this.GetComponent<Image>();
        image.sprite = normal;
    }

    public void TeclaPressionada(InputAction.CallbackContext context)
    {
        Debug.Log(context);
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
