using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/Checkerboard Rule Tile")]
public class BoardTile : TileBase
{
    [Header("Dış (Kenar) Sprite'lar")]
    public Sprite cornerTopLeft;
    public Sprite cornerTopRight;
    public Sprite cornerBottomLeft;
    public Sprite cornerBottomRight;
    public Sprite edgeTop;
    public Sprite edgeBottom;
    public Sprite edgeLeft;
    public Sprite edgeRight;

    [Header("İç Sprite'lar")]
    public Sprite innerDark;      // koyu
    public Sprite innerLight;     // az koyu

    // tilebase abstract metodu
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        bool hasTop    = tilemap.GetTile(position + Vector3Int.up)    != null;
        bool hasBottom = tilemap.GetTile(position + Vector3Int.down)  != null;
        bool hasLeft   = tilemap.GetTile(position + Vector3Int.left)  != null;
        bool hasRight  = tilemap.GetTile(position + Vector3Int.right) != null;

        bool isInterior = hasTop && hasBottom && hasLeft && hasRight;

        if (isInterior)
        {
            // çapraz desen
            bool isDark = (position.x + position.y) % 2 == 0;
            tileData.sprite = isDark ? innerDark : innerLight;
        }
        else
        {
            tileData.sprite = GetEdgeSprite(hasTop, hasBottom, hasLeft, hasRight);
        }
    }

    private Sprite GetEdgeSprite(bool top, bool bottom, bool left, bool right)
    {
        // köşeler
        if (!top && !left  && bottom && right) return cornerTopLeft;
        if (!top && !right && bottom && left)  return cornerTopRight;
        if (!bottom && !left  && top && right) return cornerBottomLeft;
        if (!bottom && !right && top && left)  return cornerBottomRight;

        // kenarlar
        if (!top    && bottom && left && right) return edgeTop;
        if (!bottom && top    && left && right) return edgeBottom;
        if (!left   && right  && top  && bottom) return edgeLeft;
        if (!right  && left   && top  && bottom) return edgeRight;

        // fallback (tek başına veya tanımsız durum)
        return cornerTopLeft;
    }
}