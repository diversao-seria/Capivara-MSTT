using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;

    private string parametroAnimator = "OpenDoor";

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

    private void OnNotePlayed(char nota)
    {
        if (nota == 'I')
        {
            Debug.Log("DoorController: Nota I detectada");
            AlternarPorta();
        }
        else if (nota == 'O')
        {
            Debug.Log("DoorController: Nota O detectada");
            AlternarPorta();
        }
    }

    public void AlternarPorta()
    {
        Debug.Log("DoorController: Alternando estado da porta");

        bool estadoAtual = anim.GetBool(parametroAnimator);

        anim.SetBool(parametroAnimator, !estadoAtual);
    }
}
