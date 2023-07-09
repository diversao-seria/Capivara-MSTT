using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fluteController : MonoBehaviour
{
    // este script só deve ser utilizado uma vez ao longo do jogo. Por este motivo, não é necessário fazer a integração com os unityevents.
    // será responsável por ativar a fila de notas, dando início à trilha da flauta
    [SerializeField] private GridController grid;
    [SerializeField] private Vector2Int coordenadasGrid;
    [SerializeField] private GameObject hud;
    public Vector2Reference coordenadasJogador;

    void Start()
    {
        // definir posição inicial com base na variável coordenadasGrid, que pode ser editada no inspetor.
        transform.position = grid.getWorldPosition(coordenadasGrid.x, coordenadasGrid.y);
    }

    public void jogadorMoveu()
    {
        // Semp´re que o jogador se move, checa se as coordenadas atuais do jogador são iguais às coordenadas atuais da flauta. Se for true, ativa a hud e destói o objeto flauta.
        if (coordenadasJogador.Value == coordenadasGrid)
        {
            hud.SetActive(true);
            Destroy(this.gameObject);
        }
    }

}
