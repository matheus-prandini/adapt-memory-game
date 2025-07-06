using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    const string kLevelKey = "CurrentLevelIndex";

    void Awake()
    {
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.SetInt(kLevelKey, 1);
        }
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll();
        DOTween.Clear(true);

        int idx = PlayerPrefs.GetInt(kLevelKey, 1) + 1;
        if (idx > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("MainMenu");
            PlayerPrefs.DeleteKey(kLevelKey);
        }
        else
        {
            PlayerPrefs.SetInt(kLevelKey, idx);
            SceneManager.LoadScene(idx);
        }
    }

    public void OnPlayFromMenu()
    {
        DOTween.KillAll();
        DOTween.Clear(true); 
        PlayerPrefs.SetInt(kLevelKey, 1);
        SceneManager.LoadScene(1);
    }

    public void ResetLevel()
    {
        DG.Tweening.DOTween.KillAll(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        DOTween.KillAll();
        DOTween.Clear(true);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
