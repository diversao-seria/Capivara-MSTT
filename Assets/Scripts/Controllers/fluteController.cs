using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fluteController : MonoBehaviour
{
    // este script s� deve ser utilizado uma vez ao longo do jogo. Por este motivo, n�o � necess�rio fazer a integra��o com os unityevents.
    // ser� respons�vel por ativar a fila de notas, dando in�cio � trilha da flauta
    [SerializeField] private GridController grid;
    [SerializeField] private Vector2Int coordenadasGrid;
    [Header("Mudanca de Parametro")]
    [SerializeField] private string nomeParametro;
    [SerializeField] private float valorParametro;
    [SerializeField] private AudioController audioController;
    public Vector2Reference coordenadasJogador;

    void Start()
    {
        // definir posi��o inicial com base na vari�vel coordenadasGrid, que pode ser editada no inspetor.
        transform.position = grid.getWorldPosition(coordenadasGrid.x, coordenadasGrid.y);
    }

    public void jogadorMoveu()
    {
        // Semp�re que o jogador se move, checa se as coordenadas atuais do jogador s�o iguais �s coordenadas atuais da flauta. Se for true, ativa a hud e dest�i o objeto flauta.
        if (coordenadasJogador.Value == coordenadasGrid)
        {
            audioController.DefinirParametrosMusica(nomeParametro, valorParametro);
            Destroy(this.gameObject);
        }
    }

}
