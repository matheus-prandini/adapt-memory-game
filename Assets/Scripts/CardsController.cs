using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CardsController : MonoBehaviour
{
    [Header("Prefabs & Transforms")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform gridTransform;

    [Header("Configuração de Fase")]
    [Tooltip("Asset de configuração desta fase.")]
    [SerializeField] private LevelConfig levelConfig;

    // estados internos
    private List<Card> allCards;
    private Sprite[]    spritePairs;
    private int[]       spriteIds;
    private Card firstSelected, secondSelected;
    private bool canSelect = true;
    private int matchesCount = 0;

    // eventos externos
    public event System.Action OnAllMatchesFound;
    public event System.Action OnCardFlipped;
    public event System.Action OnPreviewComplete;


    void Start()
    {
        if (levelConfig == null || levelConfig.spriteCategory == null)
        {
            Debug.LogError("LevelConfig ou SpriteCategory não atribuído!", this);
            enabled = false;
            return;
        }

        allCards = new List<Card>();
        PrepareAndShuffle();
        SpawnCards();
        StartCoroutine(PreviewCoroutine());
    }

    void PrepareAndShuffle()
    {
        var pool = levelConfig.spriteCategory.sprites;
        int maxPairs = Mathf.Clamp(levelConfig.pairsCount, 1, pool.Length);

        var list = new List<(Sprite, int)>();
        for (int i = 0; i < maxPairs; i++)
        {
            list.Add((pool[i], i));
            list.Add((pool[i], i));
        }

        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            var tmp = list[i]; list[i] = list[r]; list[r] = tmp;
        }

        spritePairs = list.Select(x => x.Item1).ToArray();
        spriteIds   = list.Select(x => x.Item2).ToArray();
    }

    void SpawnCards()
    {
        for (int i = 0; i < spritePairs.Length; i++)
        {
            var card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;

            var audio = card.GetComponent<CardAudio>();
            if (audio != null) audio.cardID = spriteIds[i];

            allCards.Add(card);
        }
    }

    private IEnumerator PreviewCoroutine()
    {
        canSelect = false;
        foreach (var c in allCards) c.ShowInstant();
        yield return new WaitForSeconds(levelConfig.previewDuration);
        foreach (var c in allCards) c.HideInstant();

        OnPreviewComplete?.Invoke();
        firstSelected = secondSelected = null;
        canSelect = true;
    }

    public void SetSelected(Card card)
    {
        if (!canSelect || card.isMatched) return;

        if (card.isSelected)
        {
            card.Hide();
            return;
        }

        card.Show();
        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else if (secondSelected == null)
        {
            secondSelected = card;
            canSelect = false;
            OnCardFlipped?.Invoke();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.3f);

        bool isMatch = firstSelected.iconSprite == secondSelected.iconSprite;
        if (isMatch)
        {
            matchesCount++;
            firstSelected .isMatched = true;
            secondSelected.isMatched = true;

            var cardA = firstSelected;
            var cardB = secondSelected;

            DOTween.Kill(cardA.transform);
            DOTween.Kill(cardB.transform);

            cardA.transform.localScale = cardA.OriginalScale;
            cardB.transform.localScale = cardB.OriginalScale;

            float punchMul  = 1.2f;
            float punchDur  = 0.3f;
            float returnDur = 0.15f;

            DOTween.Sequence()
                .Join(cardA.transform.DOScale(cardA.OriginalScale * punchMul, punchDur).SetEase(Ease.OutBack))
                .Join(cardB.transform.DOScale(cardB.OriginalScale * punchMul, punchDur).SetEase(Ease.OutBack))
                .AppendInterval(0.05f)
                .Join(cardA.transform.DOScale(cardA.OriginalScale, returnDur))
                .Join(cardB.transform.DOScale(cardB.OriginalScale, returnDur))
                .OnComplete(() =>
                {
                    canSelect = true;
                    firstSelected = secondSelected = null;

                    if (matchesCount == levelConfig.pairsCount)
                        OnAllMatchesFound?.Invoke();
                });
        }
        else
        {
            yield return new WaitForSeconds(1f);
            firstSelected.Hide();
            secondSelected.Hide();
            canSelect = true;
            firstSelected = secondSelected = null;
        }
    }

    void OnDestroy()
    {
        DOTween.KillAll();
    }
}
