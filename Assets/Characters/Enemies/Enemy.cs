using System;
using System.Collections;
using UnityEngine;

public class Enemy : Combatant
{
    [Header("Stats")]
    [SerializeField] private int stoppingNumber = 0;

    public static event Action<int> OnEnemyTurnEnd;
    public static event Action OnHasDied;

    private void Awake()
    {
        m_transform = transform;
        DrawInitialhHand();
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        GameManager.OnRoundEnded += HandleRoundEnded;
    }
    private void OnDisable()
    {
        PlayerCombat.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        GameManager.OnRoundEnded -= HandleRoundEnded;
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

    private void HandleRoundEnded()
    {
        canTryToHideTheSecondCard = true;
        DrawInitialhHand();
    }

    public override void Damaged()
    {
        // TODO: Play Damaged sound and animation
        Debug.Log("Enemy got damaged");
    }

    public override void Die()
    {
        // TODO: Play Death sound and animation
        ClearCards();
        OnHasDied?.Invoke();
        Destroy(gameObject);
    }
}
