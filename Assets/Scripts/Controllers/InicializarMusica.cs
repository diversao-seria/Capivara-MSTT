using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializarMusica : MonoBehaviour
{
    [SerializeField] private FMODEvents fmodEvents;
    [SerializeField] private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        audioController.PlayMusic(fmodEvents.musica);
        audioController.Setup();
    }
#if UNITY_EDITOR
    void OnGUI()
    {
        GUILayout.Box($"Current Beat = {audioController.timelineInfo.currentBeat}");
    }
#endif
}
