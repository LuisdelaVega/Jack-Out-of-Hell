using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerCombat : Combatant
{
    [Header("Properties")]
    [SerializeField] private float timeToComplete = 0.5f;

    private CinemachineVirtualCamera vcam;

    public static event Action<int> OnPlayerTurnEnd;
    public static event Action OnHasDied;
    public static event Action<int> OnPlayerBusted;

    private void Awake()
    {
        m_transform = transform;
        vcam = GameObject.FindGameObjectWithTag("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        EnemySpawner.OnPlayerMoved += HandleCombatStart;
        Enemy.OnHasDied += HandleCombatWin;
        GameManager.OnNewRoundStart += DrawInitialhHand;
    }

    private void OnDisable()
    {
        EnemySpawner.OnPlayerMoved -= HandleCombatStart;
        Enemy.OnHasDied -= HandleCombatWin;
        GameManager.OnNewRoundStart -= DrawInitialhHand;
    }

    protected void HandleCombatStart() => DrawInitialhHand();

    // This is triggered by a button
    public void HitAndShow()
    {
        Hit();
        CalculateHandTotal();

        StartCoroutine(CheckIfBust(StartCoroutine(DisplayHand(canTryToHideTheSecondCard))));
    }

    private IEnumerator CheckIfBust(Coroutine coroutine)
    {
        yield return coroutine;

        if (handTotal > 21)
        {
            if (healthBehaviour == null)
                healthBehaviour = GetComponent<HealthBehaviour>();

            OnPlayerBusted?.Invoke(healthBehaviour.DealDamage(1));
        }
    }

    public void Stay() => OnPlayerTurnEnd?.Invoke(handTotal);

    private void HandleCombatWin()
    {
        ClearCards();
        // Reposition to the middle of the room
        LeanTween.move(gameObject, new Vector2(Mathf.FloorToInt(m_transform.position.x), Mathf.CeilToInt(m_transform.position.y)), timeToComplete)
            .setEaseOutSine()
            .setOnComplete(() =>
            {
                vcam.LookAt = m_transform;
                vcam.Follow = m_transform;
            });
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
