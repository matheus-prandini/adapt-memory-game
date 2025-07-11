using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionUI : MonoBehaviour
{
    [SerializeField] private Button numbersButton;
    [SerializeField] private Button lettersButton;

    private LevelManager lm;

    void Start()
    {
        numbersButton.onClick.AddListener(() => OnModeChosen(LevelManager.GameMode.Numbers));
        lettersButton.onClick .AddListener(() => OnModeChosen(LevelManager.GameMode.Letters));
    }

    private void OnModeChosen(LevelManager.GameMode mode)
    {
        LevelManager.Instance.selectedMode = mode;
        LevelManager.Instance.StartMode();
    }
}
