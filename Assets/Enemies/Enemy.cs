using System;
using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [Header("Stats")]
    [SerializeField] private int stoppingNumber = 0;

    public static event Action<int> OnEnemyTurnEnd;

    private void Awake()
    {
        m_transform = transform;
        DrawInitialhHand();
        Invoke("Play", 1.5f);
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        GameManager.OnCombatEnded += HandleEnemyCombatLoss;
    }
    private void OnDisable()
    {
        PlayerCombat.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        GameManager.OnCombatEnded -= HandleEnemyCombatLoss;
    }

    private void HandlePlayerTurnEnd<T>(T _) => StartCoroutine(Play());

    private IEnumerator Play()
    {
        if (secondDrawnCard.IsHidden)
            secondDrawnCard.SetCardHidden(canTryToHideTheSecondCard = false, shouldAnimate: true);

        CalculateHandTotal();

        while (handTotal < stoppingNumber && IsFeelingConfident())
        {
            Hit();
            yield return StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
            CalculateHandTotal();
        }


        yield return new WaitForSeconds(1f);
        OnEnemyTurnEnd?.Invoke(handTotal);
    }

    // TODO: This won't happen like this. The enemy and player will probably be the ones signaling when combat ends once either of their health points are depleted
    private void HandleEnemyCombatLoss()
    {
        // TODO: Play a death sound and animation
        ClearCards();
        Destroy(gameObject);
    }
}
