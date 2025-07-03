using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SceneFader fader;
    [SerializeField] private string gameSceneName;

    public void OnPlayButton()
    {
        fader.FadeToScene(gameSceneName);
    }
}
