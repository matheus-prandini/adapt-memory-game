using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        fadeGroup.alpha         = 1f;
        fadeGroup.blocksRaycasts = true;
        fadeGroup.interactable   = true;
        StartCoroutine(FadeIn());
    }


    // Fade in: de 1 → 0
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 0f;

        // Muito importante: libera os cliques para o botão
        fadeGroup.blocksRaycasts = false;
        fadeGroup.interactable   = false;
    }


    // Chame este método para trocar de cena com fade
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    private IEnumerator FadeOutIn(string sceneName)
    {
        // Fade out: 0 → 1
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeGroup.alpha = elapsed / fadeDuration;
            yield return null;
        }
        fadeGroup.alpha = 1f;

        // Carrega a cena (modo Single)
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade in de volta
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = 0f;
    }
}
