// Assets/Scripts/LevelManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Prefiro usar PlayerPrefs para lembrar qual nível o jogador está
    const string kLevelKey = "CurrentLevelIndex";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Se vier do menu principal, garante que começamos em Level1
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.SetInt(kLevelKey, 1);
        }
    }

    // Chamado pela UI ou pelo GameController quando a fase termina
    public void LoadNextLevel()
    {
        int idx = PlayerPrefs.GetInt(kLevelKey, 1);
        idx++;  // próxima fase

        // Se não existir cena numerada, volta ao MainMenu
        if (idx > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene("MainMenu");
            PlayerPrefs.DeleteKey(kLevelKey);
        }
        else
        {
            PlayerPrefs.SetInt(kLevelKey, idx);
            // BuildSettings: índice de cena = ordem na lista
            SceneManager.LoadScene(idx);
        }
    }

    // Expor para botões do menu
    public void OnPlayFromMenu()
    {
        PlayerPrefs.SetInt(kLevelKey, 1);
        SceneManager.LoadScene(1); // Level1
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
