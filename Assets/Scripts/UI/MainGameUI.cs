using UnityEngine;
using UnityEngine.UIElements;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] private UIDocument mainDocument;
    private VisualElement container;
    private Button pauseButton;
    private Label scoreLabel;

    private void Awake()
    {
        container = mainDocument.rootVisualElement.Q("main-container");
        if (container == null)
        {
            Debug.LogError($"{container} objesi null.");
        } 
        scoreLabel = container.Q("score-text") as Label;
        if (scoreLabel == null)
        {
            Debug.LogError($"{scoreLabel} objesi null.");
        } 
        pauseButton = mainDocument.rootVisualElement.Q("pause-button") as Button;
        if (pauseButton == null)
        {
            Debug.LogError($"{pauseButton} objesi null.");
        } 
    }

}