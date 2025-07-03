// Assets/Scripts/GameController.cs
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Referência ao seu CardsController para saber quando zerar?
    [SerializeField] private CardsController cardsController;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        // Se não estiver linkado via Inspector, busca automático
        if (!levelManager)
            levelManager = FindObjectOfType<LevelManager>();
        if (!cardsController)
            cardsController = FindObjectOfType<CardsController>();

        // Inscreve no evento de fim de jogo (você precisará disparar isso)
        cardsController.OnAllMatchesFound += HandleAllMatchesFound;
    }

    void HandleAllMatchesFound()
    {
        // Aguarda um pouquinho antes de carregar próxima fase
        StartCoroutine(WaitAndLoadNext());
    }

    IEnumerator WaitAndLoadNext()
    {
        yield return new WaitForSeconds(1f);
        levelManager.LoadNextLevel();
    }
}
