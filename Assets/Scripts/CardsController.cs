using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;


public class CardsController : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs;
    private List<int> spriteIds;

    Card firstSelected;
    Card secondSelected;
    private List<Card> allCards;

    bool canSelect = true;

    int matchesCount = 0;

    public event System.Action OnAllMatchesFound;
    public event System.Action OnCardFlipped;
    public event System.Action OnPreviewComplete;

    void Start()
    {
        allCards = new List<Card>();
        PrepareSprites();
        CreateCards();
        StartCoroutine(PreviewCoroutine());
    }

    private void PrepareSprites()
    {
        var combined = new List<(Sprite sprite, int id)>();
        for (int id = 0; id < sprites.Length; id++)
        {
            combined.Add((sprites[id], id));
            combined.Add((sprites[id], id));
        }

        combined = combined
            .OrderBy(_ => Random.value)
            .ToList();

        spritePairs = combined.Select(x => x.sprite).ToList();
        spriteIds = combined.Select(x => x.id).ToList();
    }

    void CreateCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;

            var audio = card.GetComponent<CardAudio>();
            audio.cardID = spriteIds[i];
            allCards.Add(card);
        }
    }

    private IEnumerator PreviewCoroutine()
    {
        canSelect = false;

        foreach (var c in allCards)
            c.ShowInstant();

        yield return new WaitForSeconds(2f);

        foreach (var c in allCards)
            c.HideInstant();

        OnPreviewComplete?.Invoke();
        firstSelected = null;
        secondSelected = null;

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
            return;
        }
        if (secondSelected == null)
        {
            secondSelected = card;
            canSelect = false;
            OnCardFlipped?.Invoke();
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.3f);

        bool isMatch = firstSelected.iconSprite == secondSelected.iconSprite;
        if (isMatch)
        {
            matchesCount++;
            firstSelected.isMatched = true;
            secondSelected.isMatched = true;

            var cardA = firstSelected;
            var cardB = secondSelected;

            cardA.transform.DOKill(true);
            cardB.transform.DOKill(true);

            cardA.transform.localScale = cardA.OriginalScale;
            cardB.transform.localScale = cardB.OriginalScale;

            float punchMultiplier = 1.2f;
            float punchDuration = 0.3f;
            float returnDuration = 0.15f;

            Sequence seq = DOTween.Sequence();
            seq
            .Join(cardA.transform.DOScale(cardA.OriginalScale * punchMultiplier, punchDuration).SetEase(Ease.OutBack))
            .Join(cardB.transform.DOScale(cardB.OriginalScale * punchMultiplier, punchDuration).SetEase(Ease.OutBack))
            .AppendInterval(0.05f)
            .Join(cardA.transform.DOScale(cardA.OriginalScale, returnDuration))
            .Join(cardB.transform.DOScale(cardB.OriginalScale, returnDuration))
            .OnComplete(() =>
            {
                canSelect = true;
                firstSelected = null;
                secondSelected = null;
            });

            if (matchesCount == spritePairs.Count / 2)
                OnAllMatchesFound?.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            firstSelected.Hide();
            secondSelected.Hide();

            canSelect = true;
            firstSelected = null;
            secondSelected = null;
        }
    }


    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            Sprite temp = spriteList[i];
            int randomIndex = Random.Range(i, spriteList.Count);
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
    
    void OnDestroy()
    {
        if (gridTransform != null)
            DOTween.Kill(gridTransform);
    }
    
}