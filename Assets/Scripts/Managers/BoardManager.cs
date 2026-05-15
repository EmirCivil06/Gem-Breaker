using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Tilemap boardTilemap;
    [SerializeField] private Grid grid;
    [SerializeField] private int boardWidth = 8;
    [SerializeField] private int boardHeight = 8;
    private Vector2Int boardOrigin = Vector2Int.zero;
    private GameObject[,] cellOccupants;

    public static BoardManager Instance { get; private set; }
    public int Width => boardWidth;
    public int Height => boardHeight;
    public float CellSize => grid.cellSize.x;

    public System.Action<int> OnLinesCleared;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (grid == null)
        {
            if (boardTilemap == null)
            {
                boardTilemap = FindFirstObjectByType<Tilemap>(FindObjectsInactive.Exclude);
            }

            grid = boardTilemap.layoutGrid;
        }
        cellOccupants = new GameObject[boardWidth, boardHeight];
    }

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        Vector3Int cell = grid.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
    }

    private bool IsInBounds(Vector2Int cell)
    {
        Vector2Int local = cell - boardOrigin;
        return local.x >= 0 && local.x < boardWidth
            && local.y >= 0 && local.y < boardHeight;
    }

    private bool IsCellEmpty(Vector2Int cell)
    {
        Vector2Int local = cell - boardOrigin;
        return cellOccupants[local.x, local.y] == null;
    }

    public bool CanPlaceBlock(BlockShape shape, Vector2Int origin)
    {
        foreach (var offset in shape.cells)
        {
            Vector2Int target = origin + offset;
            if (!IsInBounds(target)) return false;
            if (!IsCellEmpty(target)) return false;
        }
        return true;
    }

    // Blok yerleştir — cell sprite'larını board'a "teslim et"
    public void PlaceBlock(BlockShape shape, Vector2Int origin, GameObject[] cellSprites)
    {
        for (int i = 0; i < shape.cells.Count; i++)
        {
            Vector2Int target = origin + shape.cells[i];
            Vector2Int local = target - boardOrigin;

            // Sprite'ı board'a bağla, pozisyonunu sabitle
            cellSprites[i].transform.SetParent(transform);
            cellSprites[i].transform.position = CellToWorld(target);
            cellOccupants[local.x, local.y] = cellSprites[i];
        }

        CheckAndClearLines();
    }

    private void CheckAndClearLines()
    {
        List<int> fullRows = new();
        List<int> fullCols = new();

        // Dolu satırları bul
        for (int y = 0; y < boardHeight; y++)
        {
            bool full = true;
            for (int x = 0; x < boardWidth; x++)
                if (cellOccupants[x, y] == null) { full = false; break; }
            if (full) fullRows.Add(y);
        }

        // Dolu sütunları bul
        for (int x = 0; x < boardWidth; x++)
        {
            bool full = true;
            for (int y = 0; y < boardHeight; y++)
                if (cellOccupants[x, y] == null) { full = false; break; }
            if (full) fullCols.Add(x);
        }

        // Temizle (satır + sütun toplamı)
        int clearedCount = fullRows.Count + fullCols.Count;
        if (clearedCount == 0) return;

        foreach (int y in fullRows)
            for (int x = 0; x < boardWidth; x++)
                ClearCell(x, y);

        foreach (int x in fullCols)
            for (int y = 0; y < boardHeight; y++)
                ClearCell(x, y);

        OnLinesCleared?.Invoke(clearedCount);
    }

    // Hücre temizleme
    private void ClearCell(int x, int y)
    {
        if (cellOccupants[x, y] != null)
        {
            // Hem referansını hem de kendisini siliyoruz
            Destroy(cellOccupants[x, y]);
            cellOccupants[x, y] = null;
        }
    }

    // Game over kontrolü: elimdeki şekiller hiçbir yere sığmıyor ise oyun biter
    public bool CanShapeFitAnywhere(BlockShape shape)
    {
        for (int x = 0; x < boardWidth; x++)
            for (int y = 0; y < boardHeight; y++)
                if (CanPlaceBlock(shape, boardOrigin + new Vector2Int(x, y)))
                    return true;
        return false;
    }
}