using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combatant
{
    private void Awake()
    {
        m_transform = transform;
    }
    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawned += InitiateCombat;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawned -= InitiateCombat;
    }

    private void InitiateCombat()
    {
        DrawInitialhHand();
    }

    public void HitAndShow()
    {
        Hit();
        CalculateHandTotal();

        StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
    }
}
