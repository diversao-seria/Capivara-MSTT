using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializarMusica : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioController.instance.PlayMusic(FMODEvents.instance.musica);
    }
}
