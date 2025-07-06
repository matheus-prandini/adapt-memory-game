using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private LevelManager levelManager;

    private CardsController cardsController;

    void Start()
    {
        victoryPanel.SetActive(false);

        cardsController = FindAnyObjectByType<CardsController>();
        if (cardsController == null)
        {
            Debug.LogError("UIManager: não encontrei CardsController na cena!");
            return;
        }

        cardsController.OnAllMatchesFound += ShowVictory;
    }

    void OnDestroy()
    {
        if (cardsController != null)
            cardsController.OnAllMatchesFound -= ShowVictory;
    }

    private void ShowVictory()
    {
        Debug.Log(">> UIManager.ShowVictory called");
        victoryPanel.transform.SetAsLastSibling();
        
        CanvasGroup cg = victoryPanel.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        victoryPanel.SetActive(true);
        DOTween.To(() => cg.alpha, a => cg.alpha = a, 1, 0.4f);
    }

    public void OnNextClicked()
    {
        victoryPanel.SetActive(false);
        levelManager.LoadNextLevel();
    }

    public void OnMenuClicked()
    {
        victoryPanel.SetActive(false);
        levelManager.BackToMenu();
    }
}
