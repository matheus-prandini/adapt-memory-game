using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rt;
    private Vector3    originalScale;

    void Awake()
    {
        rt            = GetComponent<RectTransform>();
        originalScale = rt.localScale;
    }

    // disparado quando o mouse entra na área do botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        rt.DOPunchScale(Vector3.one * 0.05f, 0.3f, vibrato: 1);
    }

    // disparado quando o mouse sai da área do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        rt.DOScale(originalScale, 0.2f);
    }
}
