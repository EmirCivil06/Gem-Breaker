using System.Collections.Generic;
using UnityEngine;

// Farklı şekillerin merkez noktasını belirlemek için statik sınıf
public static class BlockShapeGeometry
{
    // Sadece List<> değil Array ve benzeri sınıflar için liste arayüzü
    public static Vector2 GetBoundsCenter(IList<Vector2Int> cells)
    {
        if (cells == null || cells.Count == 0) return Vector2.zero;

        int minX = cells[0].x, maxX = minX;
        int minY = cells[0].y, maxY = minY;
        // Maksimum ve minimum x-y noktalarının belirlenmesi
        for (int i = 1; i < cells.Count; i++)
        {
            var c = cells[i];
            if (c.x < minX) minX = c.x; else if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y; else if (c.y > maxY) maxY = c.y;
        }
        // Orta noktanın bulunması
        return new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
    }
}