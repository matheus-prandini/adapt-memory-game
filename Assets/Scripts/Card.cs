using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CardAudio))]
public class Card : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image iconImage;         // A imagem que mostra o sprite da carta

    [Header("Sprites")]
    public Sprite hiddenIconSprite;                   // Sprite de fundo (carta virada pra baixo)
    [HideInInspector] public Sprite iconSprite;       // Sprite da face (atribuído em CardsController)

    [Header("Estado")]
    public bool isSelected = false;                   // Já está virada?
    public bool isMatched  = false;                   // Já foi casada?

    [HideInInspector] public CardsController controller;
    private CardAudio audioPlayer;

    void Awake()
    {
        // pega o CardAudio para tocar o som
        audioPlayer = GetComponent<CardAudio>();

        // começa sempre escondida
        iconImage.sprite = hiddenIconSprite;
    }

    public void OnCardClick()
    {
        // se já casou ou está bloqueado, ignora
        if (isMatched) return;
        controller?.SetSelected(this);
    }

    /// <summary>
    /// Define qual sprite esta carta mostrará na face.
    /// Chamado por CardsController.CreateCards().
    /// </summary>
    public void SetIconSprite(Sprite sprite)
    {
        iconSprite = sprite;
    }

    /// <summary>
    /// Mostra a face da carta com animação de flip e toca som.
    /// </summary>
    public void Show()
    {
        // toca o som de flip correspondente
        audioPlayer.PlayFlip();

        // anima virada: escala X até 0, troca sprite, volta a escala X=1
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
