using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int gridLength;
    private int gridHeight;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.gridLength = width;
        this.gridHeight = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, gridHeight), GetWorldPosition(gridLength, gridHeight), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(gridLength, 0), GetWorldPosition(gridLength, gridHeight), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition; 
    }

    public void setValue(int x, int y, int value)
    {
        if (x >= 0 && x < gridLength && y >= 0 && y < gridHeight)
        {
            gridArray[x, y] = value;
        }
    }

    public int returnValue(int x, int y)
    {
        if (x >= 0 && x < gridLength && y >= 0 && y < gridHeight)
        {
            return gridArray[x, y];
        }
        else
        {
            return -1;
        }
    }
}
