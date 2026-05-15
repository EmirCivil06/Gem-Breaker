using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteAlways] // Her zaman çalışmasını sağlar
public class CameraManager : MonoBehaviour
{
    // Alanlar
    [Tooltip("Yatayda görünmesini istenilen toplam dünya birimi. " +
             "Örn: 8x8 board + her iki yanda 1 birim boşluk için 8 + 1 + 1")]
    [SerializeField] private float targetWorldWidth = 10f;
    private Camera cam;
    public Transform[] spawnSlots;
    [SerializeField] private MainGameUIData mainGameUIData;

    // OnEnable
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        ManageDisplay();
    }

    // Start
    private void Start()
    {
       spawnSlots = new Transform[3]; 
    }

    // Update
    private void Update()
    {
        // Editörde güncelleme amaçlı, build'da kaldırılabilir (ne de olsa kullanıcı yatayda oynamayacak)
        ManageDisplay();
    }

    // Betiğin kalbi
    private void ManageDisplay()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (!cam.orthographic) return;
        if (Screen.width <= 0 || Screen.height <= 0) return;

        // Kameranın boyutunu güncelleme
        float aspect = (float)Screen.width / Screen.height;
        float totalWidth = targetWorldWidth + CalculateHorizontalPadding(aspect) * 2f;

        cam.orthographicSize = totalWidth / (2f * aspect);

        // Spawn slotların y değerlerini değiştirme
        foreach (Transform slot in spawnSlots) 
        {
            if (slot != null) slot.position = new Vector3(slot.position.x, CalculateSlotY(aspect));
        }
        // Ana UI'ın label'ını düzeltiyoruz
        mainGameUIData.camAspectToLabelSpacing = CalculateUILabelFlex((float)Screen.width / Screen.height);
    }

    // Yatay eksende padding hesabı
    private float CalculateHorizontalPadding(float aspect)
    {
        if (aspect < 0.5f) return 0.3f;  // Dar/uzun telefon 
        else if (aspect < 0.6f) return 0.7f;  // Telefon
        else if (aspect < 0.8f) return 1.5f;  // iPad / tablet
        else return 2.5f;  // Kare ekran
    }

    // Dikey eksende spawn slotları için y koordinatı belirleme
    private float CalculateSlotY(float aspect)
    {
        if (aspect < 0.5f || aspect < 0.65f) return -8f;  // Dar/uzun telefon + telefon
        else if (aspect < 0.8f) return -6.75f;  // iPad / tablet
        else return -7f;  // Kare ekran (veya fallback)
    }

    // Ana UI'daki labelın kötü gözükmesini engelleyecek metot
    private float CalculateUILabelFlex(float aspect)
    {
        if (aspect < 0.5f || aspect < 0.65f) return 4.75f; // Dar/uzun telefon + telefon 
        else if (aspect < 0.8f) return 18f; // iPad / tablet
        else return 10f; // Kare ekran (veya fallback)
    }
}