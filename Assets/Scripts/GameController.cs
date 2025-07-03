using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text attemptsText;

    [Header("Dependências")]
    [SerializeField] private CardsController cardsController;
    [SerializeField] private LevelManager levelManager;

    private float elapsedTime = 0f;
    private int attempts = 0;
    private bool gameRunning = true;

    void Start()
    {
        if (!cardsController) 
            cardsController = FindObjectOfType<CardsController>();

        cardsController.OnCardFlipped    += HandleCardFlipped;
        cardsController.OnAllMatchesFound += OnLevelComplete;

        UpdateTimerUI();
        UpdateAttemptsUI();
    }

    void Update()
    {
        if (!gameRunning) return;

        elapsedTime += Time.deltaTime;
        timerText.text = $"Time: {elapsedTime:F1}s";
    }

    void HandleCardFlipped()
    {
        attempts++;
        attemptsText.text = $"Attempts: {attempts}";
    }

    void OnLevelComplete()
    {
        gameRunning = false;
        StartCoroutine( WaitAndLoadNext() );
    }

    IEnumerator WaitAndLoadNext()
    {
        yield return new WaitForSeconds(1f);
        levelManager.LoadNextLevel();
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"Time: {elapsedTime:F1}s";
    }

    private void UpdateAttemptsUI()
    {
        attemptsText.text = $"Attempts: {attempts}";
    }
}
