using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Deck playingCardsDeck = null;

    [Header("Stats")]
    [SerializeField] protected int attackModifier = 0;

    [Header("Properties")]
    [SerializeField] protected int confidenceLevel = 0;
    [SerializeField] protected float timeBetweenCards = 0.25f;
    [SerializeField] protected bool canTryToHideTheSecondCard = false;
    [SerializeField] private float yOffsetForCards = 1f;

    protected Transform m_transform = null;
    protected int handTotal = 0;

    #region Card Lists
    protected readonly List<PlayingCard> nonAces = new List<PlayingCard>();
    protected readonly List<PlayingCard> aces = new List<PlayingCard>();
    // Used to keep the original order of the cards that have been drawn
    protected List<PlayingCard> allDrawnPlayingCards = new List<PlayingCard>();
    // Used to delete the Game Objects once combat is done
    protected List<GameObject> allInstantiatedCards = new List<GameObject>();
    protected PlayingCard secondDrawnCard = null;
    #endregion

    protected void DrawInitialhHand()
    {
        ClearCards();

        // Draw the first 2 cards and display them
        Hit();
        Hit();
        CalculateHandTotal();
        StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
    }

    public void Hit()
    {
        var newPlayingCard = playingCardsDeck.GetNextCard().GetComponent<PlayingCard>();
        allDrawnPlayingCards.Add(newPlayingCard);

        if (newPlayingCard.IsAnAce)
            aces.Add(newPlayingCard);
        else
            nonAces.Add(newPlayingCard);
    }

    protected void CalculateHandTotal()
    {
        // Calculate the hand total
        handTotal = 0;

        // Add all non ace cards first
        foreach (var card in nonAces)
            handTotal += card.Value;

        // Then do the aces
        for (int i = 0; i < aces.Count; i++)
        {
            if (handTotal + 11 <= 21)
                handTotal += 11;
            else
                handTotal += 1;
        }
    }

    protected IEnumerator DisplayHand(bool tryToHideTheSecondCard)
    {
        for (int i = 0; i < allDrawnPlayingCards.Count; i++)
        {
            var xOffset = ((allDrawnPlayingCards.Count / 2f - 0.5f) * -1) + (1 * i);
            var offsetPosition = new Vector2(m_transform.position.x + xOffset, m_transform.position.y + yOffsetForCards);

            PlayingCard drawnCard;

            // If the card has been instantiated, shifts it to the side
            if (allInstantiatedCards.Count > i)
            {
                drawnCard = allInstantiatedCards[i].GetComponent<PlayingCard>();
                LeanTween.move(drawnCard.gameObject, offsetPosition, timeBetweenCards / 2f);
            }
            else
            {
                drawnCard = Instantiate(allDrawnPlayingCards[i], offsetPosition, Quaternion.identity);
                allInstantiatedCards.Add(drawnCard.gameObject);
            }

            if (i == 1 && tryToHideTheSecondCard)
            {
                secondDrawnCard = drawnCard;
                secondDrawnCard.SetCardHidden(IsFeelingConfident() && allDrawnPlayingCards.Count == 2, shouldAnimate: false);
            }

            yield return new WaitForSeconds(timeBetweenCards);
        }
    }

    protected bool IsFeelingConfident() => confidenceLevel > 0;

    protected void ClearCards()
    {
        nonAces.Clear();
        aces.Clear();
        allDrawnPlayingCards.Clear();

        foreach (var card in allInstantiatedCards)
            Destroy(card);
        allInstantiatedCards.Clear();
    }
}
