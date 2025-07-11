using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public enum GameMode { Numbers, Letters }
    public GameMode selectedMode;

    [HideInInspector] public int currentLevel = 1;
    [SerializeField] private int maxNumberLevels = 8;
    [SerializeField] private int maxLetterLevels = 8;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene s)
    {
        DOTween.KillAll();
    }

    public void StartMode()
    {
        currentLevel = 1;
        LoadModeLevel();
    }

    public void LoadModeLevel()
    {
        DOTween.KillAll();

        string sceneName = $"{selectedMode}Level{currentLevel}";
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll();

        int max = selectedMode == GameMode.Numbers 
            ? maxNumberLevels 
            : maxLetterLevels;

        currentLevel++;
        if (currentLevel > max)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            LoadModeLevel();
        }
    }

    public void OnPlayFromMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("ModeSelection", LoadSceneMode.Single);
    }

    public void BackToMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ResetLevel()
    {
        DOTween.KillAll();
        LoadModeLevel();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
