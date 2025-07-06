using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text attemptsText;

    [Header("Dependências")]
    [SerializeField] private CardsController cardsController;

    private float elapsedTime = 0f;
    private int attempts = 0;
    private bool gameRunning = false;

    void Start()
    {
        if (cardsController == null)
            cardsController = FindObjectOfType<CardsController>();

        cardsController.OnPreviewComplete += HandlePreviewComplete;
        cardsController.OnCardFlipped     += HandleCardFlipped;

        UpdateTimerUI();
        UpdateAttemptsUI();
    }

    void Update()
    {
        if (!gameRunning) return;
        elapsedTime += Time.deltaTime;
        timerText.text = $"Time: {elapsedTime:F1}s";
    }

    private void HandlePreviewComplete()
    {
        elapsedTime = 0f;
        timerText.text = $"Time: {elapsedTime:F1}s";
        gameRunning = true;
    }

    private void HandleCardFlipped()
    {
        attempts++;
        attemptsText.text = $"Attempts: {attempts}";
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
