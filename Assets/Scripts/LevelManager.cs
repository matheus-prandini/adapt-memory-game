using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening.Core;

public class LevelManager : MonoBehaviour
{
    const string kLevelKey = "CurrentLevelIndex";

    void Awake()
    {
        if (transform.parent != null)
            transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene s)
    {
        DOTween.KillAll(true);
    }

    public void OnPlayFromMenu()
    {
        DOTween.KillAll(true);

        PlayerPrefs.SetInt(kLevelKey, 1);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll(true);

        int idx = PlayerPrefs.GetInt(kLevelKey, 1) + 1;
        if (idx > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            PlayerPrefs.DeleteKey(kLevelKey);
        }
        else
        {
            PlayerPrefs.SetInt(kLevelKey, idx);
            SceneManager.LoadScene(idx, LoadSceneMode.Single);
        }
    }

    public void ResetLevel()
    {
        DOTween.KillAll(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void BackToMenu()
    {
        DOTween.KillAll(true);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
