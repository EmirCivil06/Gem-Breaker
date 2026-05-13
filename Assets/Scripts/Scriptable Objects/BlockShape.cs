using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockShape", menuName = "Scriptable Objects/BlockShape")]
public class BlockShape : ScriptableObject
{
    public ShapeType shapeType;
    public List<Vector2Int> cells = new List<Vector2Int>();  
}
