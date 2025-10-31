using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Grid Tile", menuName = "Grid/Tile/Plataforma")]
public class TilePlataforma : GridTile
{
   public List<Vector2Int> posicoes = new List<Vector2Int>();
   public tipoPlataforma tipoP;
   private void Awake() 
   {
      tipo = TipoTile.Plataforma;
   }
}

public enum tipoPlataforma
{
   Grave,
   Aguda
}