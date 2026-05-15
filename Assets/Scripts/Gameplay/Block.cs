using System;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

// Blok sınıfı. Yeni input sistemine göre refactor edildi. OnMouseXxx metotları yeni input sistemi ile uyumlu olmadığından
// dolayı yeni input sisteminde yer alan EnhancedTouch kullanan metotlar ile scripti yapılandırdık.
[RequireComponent(typeof(BoxCollider2D))] 
public class Block : MonoBehaviour
{
    // Alanlar
    [SerializeField] private float reducedScaleSize = 0.75f;

    private BlockShape shape;
    private GameObject[] cellSprites;
    private Vector3 originalPosition;
    private Vector3 dragOffset;
    private Vector3 originalScale;
    private Vector3 reducedScale;
    private Camera cam;
    private bool isDragging = false;
    private int activeTouchId = -1;
    private Vector2 visualCenter;

    public BlockShape Shape => shape;

    // Awake yapılandırması
    void Awake()
    { 
        cam = Camera.main;
        InitCollider(GetComponent<BoxCollider2D>());
    }

    // Update
    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                HandleTouchBegan(touch);
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                HandleTouchMoved(touch);
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                HandleTouchEnded(touch);
        }
    }

    // Blok çağırıcının (BoardManager) yeni set oluşturduğunda her blok için çağıracağı metot
    public void Initialize(BlockShape shape, GameObject cellPrefab)
    {
        if (shape == null) throw new ArgumentNullException(nameof(shape));
        if (cellPrefab == null) throw new ArgumentNullException(nameof(cellPrefab));
        if (BoardManager.Instance == null)
            throw new InvalidOperationException("BoardManager.Instance hazır değil; Block.Initialize metodunun bu objeye ihtiyacı var.");

        // Alalara atama
        this.shape = shape;
        visualCenter = BlockShapeGeometry.GetBoundsCenter(shape.cells);
        cellSprites = BlockVisualBuilder.Build(transform, shape, cellPrefab, BoardManager.Instance.CellSize);
        originalScale = transform.localScale;
        reducedScale = new Vector3(reducedScaleSize, reducedScaleSize);
        transform.localScale = reducedScale;
        // Eğer blok yerleşemez ise eski slotuna geri koyarız
        originalPosition = transform.position;
    }

    // Bırakma eyleminde event handler
    private void HandleTouchEnded(Touch touch)
    {
        if (!isDragging || activeTouchId != touch.touchId) return;
        activeTouchId = -1;
        isDragging = false;

        Vector2Int anchor = AnchorCellFor(transform.position);

        if (BoardManager.Instance.CanPlaceBlock(shape, anchor))
        {
            BoardManager.Instance.PlaceBlock(shape, anchor, cellSprites);
            Destroy(gameObject);
        }
        else
        {
            // Eğer bloğu yerleştiremiyorsak eski slotuna geri koy
            transform.position = originalPosition;
            transform.localScale = reducedScale;
        }
    }

    // Sürükleme eyleminde event handler
    private void HandleTouchMoved(Touch touch)
    {
        if (!isDragging || activeTouchId != touch.touchId) return;

        Vector3 targetPos = ScreenToWorld(touch.screenPosition) + dragOffset;
        Vector2Int anchor = AnchorCellFor(targetPos);

        if (BoardManager.Instance.CanPlaceBlock(shape, anchor))
            transform.position = SnappedPositionFor(anchor);
        else
            transform.position = targetPos;
    }

    // Bloğa parmakla bastığımızda bas -> sürükle -> bırak döngüsünün başlaması
    private void HandleTouchBegan(Touch touch)
    {
        Vector3 worldPos = ScreenToWorld(touch.screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null || hit.gameObject != gameObject) return;

        isDragging = true;
        activeTouchId = touch.touchId;
        dragOffset = transform.position - worldPos;
        // Bloğa dokunulduğunu göstermek için 
        transform.position += Vector3.up * 0.5f;
        transform.localScale = originalScale;
    }

    // Başlangıç noktasına (0,0) oturan hücreyi bulur
    private Vector2Int AnchorCellFor(Vector3 worldPos)
    {
        float cs = BoardManager.Instance.CellSize;
        Vector3 anchorWorld = worldPos - new Vector3(visualCenter.x * cs, visualCenter.y * cs, 0);
        return BoardManager.Instance.WorldToCell(anchorWorld);
    }

    // Snap sırasında parent'ı görsel orta hizalamayla CellToWorld(anchor) + visualCenter * cellSize noktasına koyar
    private Vector3 SnappedPositionFor(Vector2Int anchor)
    {
        float cs = BoardManager.Instance.CellSize;
        return BoardManager.Instance.CellToWorld(anchor)
             + new Vector3(visualCenter.x * cs, visualCenter.y * cs, 0);
    }

    // Ekrandan dünyadaki noktaya metot 
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = cam.ScreenToWorldPoint(screenPos);
        pos.z = transform.position.z;
        return pos;
    }

    // BoxCollider2D collider'ının yapılandırılması
    private void InitCollider(BoxCollider2D boxCollider)
    {
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(3f, 3f);
    }
}