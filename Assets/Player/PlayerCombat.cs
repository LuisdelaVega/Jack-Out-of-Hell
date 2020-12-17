using System;
using UnityEngine;

public class PlayerCombat : Combatant
{
    [Header("Properties")]
    [SerializeField] private float timeToComplete = 0.5f;

    public static event Action<int> OnPlayerTurnEnd;

    private void Awake() => m_transform = transform;
    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawned += InitiateCombat;
        GameManager.OnCombatEnded += HandlePlayerCombatWin;
    }
    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawned -= InitiateCombat;
        GameManager.OnCombatEnded -= HandlePlayerCombatWin;
    }

    private void InitiateCombat() => DrawInitialhHand();

    public void HitAndShow()
    {
        Hit();
        CalculateHandTotal();

        StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
    }

    public void Stay() => OnPlayerTurnEnd?.Invoke(handTotal);

    private void HandlePlayerCombatWin()
    {
        ClearCards();
        LeanTween.move(gameObject, new Vector2(Mathf.FloorToInt(m_transform.position.x), Mathf.CeilToInt(m_transform.position.y)), timeToComplete)
            .setEaseOutSine();
    }
}
