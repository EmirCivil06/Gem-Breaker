using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    // Var olan blok şekilleri ve renkli bloklar
    [SerializeField] private List<BlockShape> availableShapes;
    [SerializeField] private List<GameObject> availableBlocks;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Transform[] spawnSlots; // 3 adet boş transform

    private Block[] currentBlocks;

    private void Start()
    {
        currentBlocks = new Block[spawnSlots.Length];
        // İşlem yapılacak öğeleri kopyala veya listeyi değiştirme mantığını güncelle
        List<BlockShape> newShapes = new(3);

        foreach (BlockShape shape in availableShapes)
        {
            if (shape == null) continue; // Null kontrolü ekle

            var sType = shape.shapeType;
            if (sType == ShapeType.SimilarToLine || sType == ShapeType.Z)
            {
                newShapes.Add(VectorExtensions.Rotated90(shape));
            }
            // Cube ise hiçbir şey yapmıyoruz
            else if (sType != ShapeType.Cube) 
            {
                newShapes.Add(VectorExtensions.Rotated90(shape));
                newShapes.Add(VectorExtensions.Rotated180(shape));
                newShapes.Add(VectorExtensions.Rotated270(shape));
            }
        }

        // Yeni şekilleri orijinal listeye güvenli bir şekilde ekle
        availableShapes.AddRange(newShapes);
        SpawnNewSet();
    }

    // Update
    private void Update()
    {
        // Şu anki blokların hepsi null olursa yeni set spawnlama
        if (currentBlocks == null) return;

        bool allUsed = true;
        for (int i = 0; i < currentBlocks.Length; i++)
        {
            if (currentBlocks[i] != null) { allUsed = false; break; }
        }
        if (allUsed) SpawnNewSet();
    }

    // Yeni blok seti spawnlama
    private void SpawnNewSet()
    {
        for (int i = 0; i < spawnSlots.Length; i++)
        {
            var shape = availableShapes[Random.Range(0, availableShapes.Count)];
            var blockColor = availableBlocks[Random.Range(0, availableBlocks.Count)];

            var block = Instantiate(blockPrefab, spawnSlots[i].position, Quaternion.identity);
            block.Initialize(shape, blockColor);
            currentBlocks[i] = block;
        }
    }
}