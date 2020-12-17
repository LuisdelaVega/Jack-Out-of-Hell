using UnityEngine;

public class Enemy : Combatant
{
    [Header("Stats")]
    [SerializeField] private int stoppingNumber = 0;

    private void Awake()
    {
        m_transform = transform;
        DrawInitialhHand();
        Invoke("Play", 1.5f);
    }

    private void Play()
    {
        if (secondDrawnCard.IsHidden)
            secondDrawnCard.SetCardHidden(canTryToHideTheSecondCard = false, shouldAnimate: true);

        CalculateHandTotal();

        while (handTotal < stoppingNumber && IsFeelingConfident())
        {
            Hit();
            CalculateHandTotal();
        }

        StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
    }
}
