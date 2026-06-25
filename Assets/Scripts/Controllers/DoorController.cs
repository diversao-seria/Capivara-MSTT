using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GridController grid;
    [SerializeField] private Vector2Int posicao;
    [SerializeField] private char notaMexe;
    public bool aberta = false;
    public bool vertical = false;
    [SerializeField] private AudioController audioController;
    [SerializeField] private FMODEvents fmodEvents;
    private EventReference somAbre;
    private EventReference somFecha;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GridController.NotePlayed += OnNotePlayed;
    }

    private void OnDisable()
    {
        GridController.NotePlayed -= OnNotePlayed;
    }

    void Start()
    {
        transform.position = grid.getWorldPosition(posicao.x, posicao.y);
        transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
        if (aberta)
        {
            OnNotePlayed(notaMexe);
        }
        if (vertical)
        {
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
        }
        
    }

    private void OnNotePlayed(char nota)
    {
        Debug.Log("NOTE PLAYED");
        if (nota == 'I')
        {
            Debug.Log("DoorController: Nota I detectada");
            somAbre = fmodEvents.porta_abre_agudo;
            somFecha = fmodEvents.porta_fecha_agudo;
            AlternarPorta("DoorHigh", nota);
        }
        else if (nota == 'O')
        {
            Debug.Log("DoorController: Nota O detectada");
            somAbre = fmodEvents.porta_abre_grave;
            somFecha = fmodEvents.porta_fecha_grave;
            AlternarPorta("DoorLow", nota);
        }
    }

    public void AlternarPorta(string parametroAnimator, char nota)
    {
        Debug.Log("DoorController: Alternando estado da porta");
        Debug.Log(nota);

        bool estadoAtual = anim.GetBool(parametroAnimator);

        anim.SetBool(parametroAnimator, !estadoAtual);

        if (nota == notaMexe) 
        {
            if (estadoAtual)
            {
                audioController.tocarOneShot(somFecha);
            }
            else
            {
                audioController.tocarOneShot(somAbre);
            }

            grid.OnDoorSwitch(posicao);
        }
    }
}
