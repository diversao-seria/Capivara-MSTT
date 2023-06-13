using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataController : MonoBehaviour
{
    public IntReference tentativas;

    public void onGameOver()
    {
        tentativas.Value += 1;
    }

    public void jogadorVenceuNivel()
    {
        tentativas.Value = 1;
    }
}
