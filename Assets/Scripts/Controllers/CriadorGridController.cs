using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CriadorGridController : MonoBehaviour
{
    [SerializeField] private GridSO gridSO;
    [SerializeField] private GridTile tile;

    // TODO: veriricar questões de herança
    [SerializeField] private TilePlataforma tileP;
    [SerializeField] private List<Vector2Int> posicoesPlataforma;
    private GameObject previewTile;
    private Vector2Int posicaoAtualCursor = Vector2Int.zero;
    private bool plataformaFim = false;

    private int cellSize = 1;
    private PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();
    }  
    void OnEnable()
    {
        playerInputActions.UI.Mouse.performed += CliqueMouse;
    }
    void OnDisable()
    {
        playerInputActions.UI.Mouse.performed -= CliqueMouse;
    }
    // Start is called before the first frame update
    void Start()
    {
        cellSize = 1;

        gridSO.Inicializar();
        
        for (int x = 0; x < gridSO.gridList.Count; x++)
        {
            for (int y = 0; y < gridSO.gridList[0].Count; y++)
            {
                GameObject instance = Instantiate(tile.prefab, getWorldPosition(x, y), Quaternion.identity);
                gridSO.AdicionarObjeto(x, y, instance);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInGrid(GetMousePosition()))
        {
            Vector2Int posicaoMouse = GetMousePosition();
            if (posicaoMouse != posicaoAtualCursor)
            {
                if (previewTile != null)
                {
                    Destroy(previewTile);
                }
                previewTile = Instantiate(tile.prefab, getWorldPosition(posicaoMouse.x, posicaoMouse.y), Quaternion.identity);
                posicaoAtualCursor = posicaoMouse;   
            }                    
        }
        else
        {
            if (previewTile != null)
            {
                Destroy(previewTile);
            }
        }
    }
    public Vector3 getWorldPosition(int x, int y)
    {
        // Converte coordenadas (X, Y) em coordenadas da Unity
        return gridSO.GetWorldPosition(Vector3.zero, new Vector2Int(x, y), cellSize) + new Vector3(cellSize, 0, cellSize) * 0.5f;
    }

    private void GetXY (Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.z / cellSize);
    }
    public void CliqueMouse(InputAction.CallbackContext context)
    {
        Vector2Int posicaoMouse = GetMousePosition();    
 
        if (posicaoMouse.x < gridSO.gridList.Count && posicaoMouse.y < gridSO.gridList[0].Count && posicaoMouse.x > -1 && posicaoMouse.y > -1)
        {
            if (tile.tipo == TipoTile.Plataforma)
            {
                if (!plataformaFim)
                {
                    posicoesPlataforma.Add(new Vector2Int(posicaoMouse.x, posicaoMouse.y));
                    plataformaFim = true;
                }
                else
                {
                    posicoesPlataforma.Add(new Vector2Int(posicaoMouse.x, posicaoMouse.y));
                    plataformaFim = false;

                    tileP.posicoes = posicoesPlataforma;
                    // tileP.tipoP = tipoPlataforma.

                    gridSO.AdicionarTile(posicaoMouse.x, posicaoMouse.y, tileP);
                    Destroy(gridSO.objectList[posicoesPlataforma[0].x][posicoesPlataforma[0].y]);
                    GameObject instance = Instantiate(tile.prefab, getWorldPosition(posicoesPlataforma[0].x, posicoesPlataforma[0].y), Quaternion.identity);
                    gridSO.AdicionarObjeto(posicoesPlataforma[0].x, posicoesPlataforma[0].y, instance);
                    posicoesPlataforma.Clear();
                }  
            }
            else
            {
                gridSO.AdicionarTile(posicaoMouse.x, posicaoMouse.y, tile);
                Debug.Log(gridSO.objectList[posicaoMouse.x][posicaoMouse.y]);
                Destroy(gridSO.objectList[posicaoMouse.x][posicaoMouse.y]);
                GameObject instance = Instantiate(tile.prefab, getWorldPosition(posicaoMouse.x, posicaoMouse.y), Quaternion.identity);
                gridSO.AdicionarObjeto(posicaoMouse.x, posicaoMouse.y, instance);
            }    
        }
    }

    public Vector2Int GetMousePosition()
    {
        int gx, gy;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10;
        Vector3 mouseWPos = Camera.main.ScreenToWorldPoint(mousePos);
        GetXY(mouseWPos, out gx, out gy);
        return new Vector2Int(gx, gy);
    }
    public bool IsInGrid(Vector2Int pos)
    {
        if (pos.x < gridSO.gridList.Count && pos.y < gridSO.gridList[0].Count && pos.x > -1 && pos.y > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AumentarGridX()
    {
        gridSO.objectList.Add(new List<GameObject>());
        for (int i = 0; i < gridSO.gridList[0].Count; i++)
        {
            GameObject instance = Instantiate(gridSO.tileVazio.prefab, getWorldPosition(gridSO.objectList.Count - 1, i), Quaternion.identity);
            gridSO.objectList[gridSO.objectList.Count - 1].Add(instance);
        }
    }

    public void ReduzirGridX()
    {
        if (gridSO.objectList.Count > 1)
        {
            for (int i = 0; i < gridSO.gridList[0].Count; i++)
            {
                Destroy(gridSO.objectList[gridSO.objectList.Count - 1][i]);
            }
            gridSO.objectList.RemoveAt(gridSO.objectList.Count - 1);
        }
        // Debug.Log(gridSO.objectList.Count + ", " + gridSO.gridList.Count);
    }

    public void AumentarGridY()
    {
        for (int i = 0; i < gridSO.objectList.Count; i++)
        {
            GameObject instance = Instantiate(gridSO.tileVazio.prefab, getWorldPosition(i, gridSO.objectList[i].Count), Quaternion.identity);
            gridSO.objectList[i].Add(instance);
        }
        Debug.Log(gridSO.objectList.Count + ", " + gridSO.gridList[0].Count);
    }

    public void ReduzirGridY()
    {
        if (gridSO.objectList[0].Count > 1)
        {
            for (int i = 0; i < gridSO.objectList.Count; i++)
            {
                Destroy(gridSO.objectList[i][gridSO.objectList[i].Count - 1]);
                gridSO.objectList[i].RemoveAt(gridSO.objectList[i].Count - 1);
            }
        }  
    }
}
