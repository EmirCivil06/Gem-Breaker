using System;
using UnityEngine;
// Şekillerin görsel olarak oluşturulamsı için yardımcı sınıf ve metot
public static class BlockVisualBuilder
{
    public static GameObject[] Build(Transform parent, BlockShape shape, GameObject cellPrefab, float cellSize)
    {
        if (parent == null) throw new ArgumentNullException(nameof(parent));
        if (shape == null) throw new ArgumentNullException(nameof(shape));
        if (cellPrefab == null) throw new ArgumentNullException(nameof(cellPrefab));

        // Orta nokta
        Vector2 center = BlockShapeGeometry.GetBoundsCenter(shape.cells);

        // Hücre spriteları
        var cellSprites = new GameObject[shape.cells.Count];
        for (int i = 0; i < shape.cells.Count; i++)
        {
            // Offset ve merkeze göre hesaplama
            var offset = shape.cells[i];
            var cellObj = UnityEngine.Object.Instantiate(cellPrefab, parent);
            cellObj.transform.localPosition = new Vector3(
                (offset.x - center.x) * cellSize,
                (offset.y - center.y) * cellSize,
                0);
            cellSprites[i] = cellObj;
        }
        return cellSprites;
    }
}