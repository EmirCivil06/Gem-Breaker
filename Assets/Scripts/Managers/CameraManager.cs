using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways] // her zaman çalışır
public class CameraManager : MonoBehaviour
{
    [Tooltip("Yatayda görünmesini istenilen toplam dünya birimi. " +
             "Örn: 8x8 board + her iki yanda 1 birim boşluk için 8 + 1 + 1")]
    [SerializeField] private float targetWorldWidth = 10f;
    private Camera cam;
    public Transform[] spawnSlots;

    // OnEnable
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        ManageDisplay();
    }

    private void Start()
    {
       spawnSlots = new Transform[3]; 
    }

    // Update
    private void Update()
    {
        // editörde güncelleme amaçlı, build'da kaldırılabilir (ne de olsa kullanıcı yatayda oynamayacak)
        ManageDisplay();
    }

    // betiğin kalbi
    private void ManageDisplay()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (!cam.orthographic) return;

        // kameranın boyutunu güncelleme
        float aspect = (float)Screen.width / Screen.height;
        float totalWidth = targetWorldWidth + CalculateHorizontalPadding(aspect) * 2f;

        cam.orthographicSize = totalWidth / (2f * aspect);

        // spawn slotların y değerlerini değiştirme
        foreach (Transform slot in spawnSlots) 
        {
            if (slot != null) slot.position = new Vector3(slot.position.x, CalculateSlotY(aspect));
        }
    }

    // yatay eksende padding hesabı
    private float CalculateHorizontalPadding(float aspect)
    {
        if (aspect < 0.5f) return 0.3f;  // dar/uzun telefon — minimum padding
        else if (aspect < 0.6f) return 0.7f;  // telefon
        else if (aspect < 0.8f) return 1.5f;  // iPad / tablet
        else return 2.5f;  // kare ekran
    }

    // dikey eksende spawn slotları için y koordinatı belirleme
    private float CalculateSlotY(float aspect)
    {
        if (aspect < 0.5f) return -8f;  // dar/uzun telefon — minimum padding
        else if (aspect < 0.6f) return -8f;  // telefon
        else if (aspect < 0.8f) return -6.75f;  // iPad / tablet
        else return -6.75f;  // kare ekran
    }
}