using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializarAudioManager : MonoBehaviour
{
    [SerializeField] private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController.criarDialogueCallback();
        // Debug.Log("Done ;D");
    }
}
