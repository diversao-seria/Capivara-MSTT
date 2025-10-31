using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoTile
{
    Plataforma,
    BotaoGrave,
    BotaoAgudo,
    Fim,
    Inicio,
    Andavel,
    NaoAndavel,
    Vazio
}

[CreateAssetMenu(fileName = "New Grid Tile", menuName = "Grid/Tile")]
public class GridTile : ScriptableObject
{
    public TipoTile tipo;
    public GameObject prefab;
}
