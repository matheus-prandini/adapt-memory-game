using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text   victoryMessage;

    [Header("Dependências de Jogo")]
    [SerializeField] private GameObject gameLayoutRoot;

    private GameController  gameController;
    private CardsController cardsController;

    void Awake()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    void Start()
    {
        // captura o GameController
        gameController = FindObjectOfType<GameController>();
        if (gameController == null)
            Debug.LogError("UIManager: não encontrei GameController!");

        // captura o CardsController e assina
        cardsController = FindObjectOfType<CardsController>();
        if (cardsController == null)
            Debug.LogError("UIManager: não encontrei CardsController!");
        else
            cardsController.OnAllMatchesFound += ShowVictory;
    }

    void OnDestroy()
    {
        if (cardsController != null)
            cardsController.OnAllMatchesFound -= ShowVictory;
    }

    private void ShowVictory()
    {
        // trava interações com o tabuleiro
        cardsController.enabled = false;
        if (gameLayoutRoot != null)
            gameLayoutRoot.SetActive(false);

        if (gameController == null) return;

        // monta mensagem
        float t = gameController.ElapsedTime;
        int   a = gameController.Attempts;
        victoryMessage.text = $"Parabéns!\nVocê levou {t:F1}s e {a} tentativas.";

        // mostra overlay
        victoryPanel.transform.SetAsLastSibling();
        victoryPanel.SetActive(true);
    }

    // agora chama o singleton direto
    public void OnNextClicked()
    {
        victoryPanel.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
    }

    public void OnMenuClicked()
    {
        victoryPanel.SetActive(false);
        LevelManager.Instance.BackToMenu();
    }
}
