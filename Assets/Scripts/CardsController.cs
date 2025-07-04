using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        spriteIds   = combined.Select(x => x.id).ToList();
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
        firstSelected  = null;
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
        if (firstSelected.iconSprite == secondSelected.iconSprite)
        {
            matchesCount++;
            firstSelected.isMatched = true;
            secondSelected.isMatched = true;
            if (matchesCount == spritePairs.Count / 2)
            {
                Debug.Log("All matches found!");
                PrimeTween.Sequence.Create()
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one * 1.2f, 0.5f, ease: PrimeTween.Ease.OutBack))
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one, 0.1f));
                OnAllMatchesFound?.Invoke();
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            firstSelected.Hide();
            secondSelected.Hide();
        }
        firstSelected = null;
        secondSelected = null;
        canSelect = true;
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
    
}