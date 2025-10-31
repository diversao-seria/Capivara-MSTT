using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    public int coletavel; // Trocar de "INT" para SO Coletavel. Cada tile só pode ter 2 estados: com coletável ou sem. Não deve ser possível ter mais de um coletável por tile.
    public GridTile tile;
    public GridCell (int _coletavel, GridTile _tile)
    {
        coletavel = _coletavel;
        tile = _tile;
    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Grid", menuName = "Grid/Grid")]
public class GridSO : ScriptableObject
{
    public int gridLength = 3;
    public int gridHeight = 3;
    public List<List<GridCell>> gridList;
    public List<List<GameObject>> objectList;
    public GridCell[,] gridArray;
    public GridTile tileVazio;

    private void Awake() 
    {
        gridArray = new GridCell[gridLength, gridHeight];
    }
    public void AdicionarTile(int x, int y, GridTile tile)
    {
        // gridArray[x, y] = new GridCell(0, tile);
        gridList[x][y] = new GridCell(0, tile);    
    }
    public void Inicializar()
    {
        // gridArray = new GridCell[gridLength, gridHeight];
        gridList = new List<List<GridCell>>();
        objectList = new List<List<GameObject>>();
        for (int x = 0; x < gridLength; x++)
        {
            gridList.Add(new List<GridCell>());
            objectList.Add(new List<GameObject>());

            for (int y = 0; y < gridHeight; y++)
            {
                // AdicionarTile(x, y, tileVazio);
                gridList[x].Add(new GridCell(0, tileVazio));
                objectList[x].Add(null);
            }
        }
    }
    public void AdicionarObjeto(int x, int y, GameObject objeto)
    {
        objectList[x][y] = objeto;
    }

    public Vector3 GetWorldPosition(Vector3 origin, Vector2Int gridCoords, float cellSize)
    {
        return new Vector3(gridCoords.x, 0, gridCoords.y) * cellSize + origin;
    }

    public void AumentarGridX()
    {
        gridList.Add(new List<GridCell>());
        for (int i = 0; i < gridList[0].Count; i++)
        {
            gridList[gridList.Count - 1].Add(new GridCell(0, tileVazio));
        }
    }

    public void ReduzirGridX()
    {
        if (gridList.Count > 1)
        {
            gridList[gridList.Count - 1].Clear();
            gridList.RemoveAt(gridList.Count - 1);
        }     
    }

    public void AumentarGridY()
    {
        for (int i = 0; i < gridList.Count; i++)
        {
            gridList[i].Add(new GridCell(0, tileVazio));
            Debug.Log(gridList[i]);
        }
    }

    public void ReduzirGridY()
    {
        if (gridList[0].Count > 1)
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].RemoveAt(gridList[i].Count - 1);
            }
        }
    }
    
}
