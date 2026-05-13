using UnityEngine;

public static class VectorExtensions
{
    // 90 derece döndürür
    public static Vector2Int Rotate90(this Vector2Int v)
    {
        return new Vector2Int(v.y, -v.x);
    }

    // 180 derece döndürür
    public static Vector2Int Rotate180(this Vector2Int v)
    {
        return new Vector2Int(-v.x, -v.y);
    }

    // 270 derece saat yönünde döndürür
    public static Vector2Int Rotate270(this Vector2Int v)
    {
        return new Vector2Int(-v.y, v.x);
    }

    // bloğu bütün olarak 90 derece döndür
    public static BlockShape Rotated90(BlockShape shape) 
    {
        BlockShape newShape = ScriptableObject.CreateInstance<BlockShape>();
        newShape.shapeType = ShapeType.Duplicate;
        foreach (Vector2Int v in shape.cells) 
        {
            newShape.cells.Add(Rotate90(v));
        }
        return newShape;
    }

    // bloğu bütün olarak 180 derece döndür
    public static BlockShape Rotated180(BlockShape shape)
    {
        BlockShape newShape = ScriptableObject.CreateInstance<BlockShape>();
        newShape.shapeType = ShapeType.Duplicate;
        foreach(Vector2Int v in shape.cells)
        {
            newShape.cells.Add(Rotate180(v));
        }
        return newShape;
    }

    // bloğu bütün olarak 270 derece döndür
    public static BlockShape Rotated270(BlockShape shape)
    {
        BlockShape newShape = ScriptableObject.CreateInstance<BlockShape>();
        newShape.shapeType = ShapeType.Duplicate;
        foreach(Vector2Int v in shape.cells)
        {
            newShape.cells.Add(Rotate270(v));
        }
        return newShape;
    }
}