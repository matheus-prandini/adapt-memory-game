using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        // 1) Faça este GameObject persistir entre as cenas
        if (transform.parent != null) transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        // 2) Inicialize o DOTween uma vez, sem criar DOTweenComponent persistente
        DOTween.Init(useSafeMode: true, recycleAllByDefault: false, logBehaviour: LogBehaviour.ErrorsOnly);

        // 3) Sempre que uma cena for descarregada, destrua o DOTweenComponent e limpe
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene sc)
    {
        // Encontra e destrói o GameObject [DOTween] que sobreviveu
        var dtc = FindAnyObjectByType<DG.Tweening.Core.DOTweenComponent>();
        if (dtc != null) Destroy(dtc.gameObject);

        // Mata todos os tweens e limpa o cache interno
        DOTween.KillAll();
        DOTween.Clear(true);
    }

    void OnDestroy()
    {
        // Desinscreve para evitar leaks se o Bootstrapper for destruído
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
