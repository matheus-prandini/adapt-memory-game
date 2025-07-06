using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text   victoryMessage;

    [Header("Dependências")]
    [SerializeField] private LevelManager levelManager;

    private GameController  gameController;
    private CardsController cardsController;

    void Awake()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    void Start()
    {
        gameController = FindAnyObjectByType<GameController>();
        if (gameController == null)
            Debug.LogError("UIManager: não encontrei nenhum GameController na cena!");

        cardsController = FindAnyObjectByType<CardsController>();
        if (cardsController == null)
        {
            Debug.LogError("UIManager: não encontrei nenhum CardsController na cena!");
        }
        else
        {
            cardsController.OnAllMatchesFound += ShowVictory;
        }
    }

    void OnDestroy()
    {
        if (cardsController != null)
            cardsController.OnAllMatchesFound -= ShowVictory;
    }

    private void ShowVictory()
    {
        if (gameController == null)
            return;

        float t = gameController.ElapsedTime;
        int   a = gameController.Attempts;
        victoryMessage.text = $"Parabéns!\nVocê levou {t:F1} segundos\ne {a} tentativas para finalizar.";

        victoryPanel.SetActive(true);
    }

    public void OnNextClicked()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
        if (levelManager != null)
            levelManager.LoadNextLevel();
    }

    public void OnMenuClicked()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
        if (levelManager != null)
            levelManager.BackToMenu();
    }
}
