using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs;

    Card firstSelected;
    Card secondSelected;

    bool canSelect = true;

    int matchesCount = 0;


    void Start()
    {
        PrepareSprites();
        CreateCards();
    }

    private void PrepareSprites()
    {
        spritePairs = new List<Sprite>();
        foreach (var sprite in sprites)
        {
            spritePairs.Add(sprite);
            spritePairs.Add(sprite);
        }

        ShuffleSprites(spritePairs);
    }

    void CreateCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;
        }
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