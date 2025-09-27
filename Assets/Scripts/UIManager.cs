using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

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
        gameController = FindFirstObjectByType<GameController>();
        if (gameController == null)
            Debug.LogError("UIManager: não encontrei GameController!");

        // captura o CardsController e assina
        cardsController = FindFirstObjectByType<CardsController>();
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
        int l = LevelManager.Instance.currentLevel;
        float t = gameController.ElapsedTime;
        int   a = gameController.Attempts;
        victoryMessage.text = $"Parabéns!\nVocê finalizou o level {l} - levou {t:F1}s e {a} tentativas.";

        // mostra overlay
        victoryPanel.transform.SetAsLastSibling();
        victoryPanel.SetActive(true);

        // envia evento de jogo
        StartCoroutine(SendGameEvent(l, t, a));
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

    IEnumerator SendGameEvent(int level, float elapsedTime, int numAttempts)
    {
        string backendUrl = "https://adapt2learn-895112363610.us-central1.run.app/api/events/game";
        string levelStr = level.ToString();
        string elapsedStr = elapsedTime.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);
        string numAttemptsStr = numAttempts.ToString();
        string json = $"{{" +
                  $"\"event_type\":\"finish_level\"," +
                  $"\"game_id\":\"{WebGLManager.Instance.GameId}\"," +
                  $"\"payload\":{{" +
                  $"\"level\":{levelStr}," +
                  $"\"time\":{elapsedStr}," +
                  $"\"attempts\":{numAttemptsStr}" +
                  $"}}}}";

        Debug.Log(json);
        Debug.Log(WebGLManager.Instance.AuthToken);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(backendUrl, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + WebGLManager.Instance.AuthToken);

            yield return req.SendWebRequest();

            Debug.Log(req.result);
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Erro ao enviar evento: {req.error}. Resp: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"Evento enviado com sucesso: {req.downloadHandler.text}");
            }
        }
    }
}
