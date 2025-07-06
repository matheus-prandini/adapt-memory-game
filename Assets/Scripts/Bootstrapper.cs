using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Bootstrapper : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DOTween.Init(useSafeMode: true, recycleAllByDefault: false, logBehaviour: LogBehaviour.ErrorsOnly);
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene s)
    {
        var dtc = FindAnyObjectByType<DG.Tweening.Core.DOTweenComponent>();
        if (dtc != null) Destroy(dtc.gameObject);
        DOTween.KillAll();
        DOTween.Clear(true);
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

}
