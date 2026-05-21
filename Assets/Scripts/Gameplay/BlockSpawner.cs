using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public static BlockSpawner Instance { get; private set; }

    // Var olan blok şekilleri ve renkli bloklar
    [SerializeField] private List<BlockShape> availableShapes;
    [SerializeField] private List<GameObject> availableBlocks;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Transform[] spawnSlots; // 3 adet boş transform

    // Aktif blokların yığıtı; yerleştirilen (destroy edilen) bloklar her frame ayıklanır
    private Stack<Block> currentBlocks = new();

    // GameManager'ın "elimdeki bloklar" üzerinde okuma yapabilmesi için dışarı açıyoruz
    public IReadOnlyCollection<Block> CurrentBlocks => currentBlocks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
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

    private void Update()
    {
        // Yerleştirilen bloklar Destroy edilir; yığıttaki ölü referansları temizle
        PruneDestroyed();

        // Yığıt tamamen boşaldıysa yeni set spawnla
        if (currentBlocks.Count == 0) SpawnNewSet();
    }

    // Destroy edilmiş Unity nesnelerini yığıttan ayıkla. Stack rastgele eleman çıkarmaya
    // izin vermediği için, sadece yaşayanları içeren yeni bir yığıt oluşturuyoruz.
    private void PruneDestroyed()
    {
        if (currentBlocks.Count == 0) return;

        bool anyDestroyed = false;
        foreach (var b in currentBlocks)
        {
            if (b == null) { anyDestroyed = true; break; }
        }
        if (!anyDestroyed) return;

        var alive = new Stack<Block>(currentBlocks.Count);
        foreach (var b in currentBlocks)
        {
            if (b != null) alive.Push(b);
        }
        currentBlocks = alive;
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
            currentBlocks.Push(block);
        }
    }
}
