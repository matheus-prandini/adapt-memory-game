using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Card : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public Sprite hiddenIconSprite;
    public Sprite iconSprite;

    public bool isSelected;

    public bool isMatched;

    public CardsController controller;
    

    public void OnCardClick()
    {
        if (isMatched) return;
        controller?.SetSelected(this);
    }


    public void SetIconSprite(Sprite sprite)
    {
        iconSprite = sprite;
    }

    public void Show()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0f, 0.2f))
       .AppendCallback(() =>
       {
           iconImage.sprite = iconSprite;
           isSelected = true;
       })
       .Append(transform.DOScaleX(1f, 0.2f));
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0f, 0.2f))
        .AppendCallback(() =>
        {
            iconImage.sprite = hiddenIconSprite;
            isSelected = false;
        })
        .Append(transform.DOScaleX(1f, 0.2f));
    }

}