using System;
using UnityEngine;

public class PlayerCombat : Combatant
{
    [Header("Properties")]
    [SerializeField] private float timeToComplete = 0.5f;

    public static event Action<int> OnPlayerTurnEnd;
    public static event Action OnHasDied;

    private void Awake() => m_transform = transform;
    private void OnEnable()
    {
        EnemySpawner.OnPlayerMoved += HandleCombatStart;
        Enemy.OnHasDied += HandleCombatWin;
        GameManager.OnRoundEnded += DrawInitialhHand;
    }
    private void OnDisable()
    {
        EnemySpawner.OnPlayerMoved -= HandleCombatStart;
        Enemy.OnHasDied -= HandleCombatWin;
        GameManager.OnRoundEnded -= DrawInitialhHand;
    }

    protected void HandleCombatStart() => DrawInitialhHand();
    public void HitAndShow()
    {
        Hit();
        CalculateHandTotal();

        StartCoroutine(DisplayHand(canTryToHideTheSecondCard));
    }

    public void Stay() => OnPlayerTurnEnd?.Invoke(handTotal);

    private void HandleCombatWin()
    {
        ClearCards();
        // Reposition to the middle of the room
        LeanTween.move(gameObject, new Vector2(Mathf.FloorToInt(m_transform.position.x), Mathf.CeilToInt(m_transform.position.y)), timeToComplete)
            .setEaseOutSine();
    }

    public override void Damaged()
    {
        // TODO: Play Damaged sound and animation
        Debug.Log("Player got damaged");
    }

    public override void Die()
    {
        // TODO: Play Death sound and animation
        OnHasDied?.Invoke();
        Destroy(gameObject);
    }
}
