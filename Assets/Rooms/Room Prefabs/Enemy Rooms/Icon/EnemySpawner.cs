using System;
using UnityEngine;
using Cinemachine;

public class EnemySpawner : MonoBehaviour, IRoomTrigger
{
    [Header("References")]
    [SerializeField] private Deck enemyDeck = null;
    [SerializeField] private Deck playingCardsDeck = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    [Header("Posiiton Offset"), Range(0f, 0.99f)]
    [SerializeField] private float offset = 0.75f;

    [Header("Time")]
    [SerializeField] private float timeToComplete = 1f;

    private CinemachineVirtualCamera vcam;
    private Transform m_transform;

    public static event Action<Combatant> OnEnemySpawned;
    public static event Action OnPlayerMoved;

    private void Awake()
    {
        m_transform = transform;
        vcam = GameObject.FindGameObjectWithTag("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable() => Enemy.OnHasDied += HandleCombatEnd;
    private void OnDisable() => Enemy.OnHasDied -= HandleCombatEnd;

    public void TriggerRoom(Transform player)
    {
        var enemy = Instantiate(enemyDeck.GetNextCard(), new Vector2(player.position.x - offset, player.position.y + offset), Quaternion.identity);

        if (enemy.TryGetComponent<Combatant>(out var combatant))
            OnEnemySpawned?.Invoke(combatant);

        // Move the player to the oposite corner of the room
        LeanTween.move(player.gameObject, new Vector2(player.position.x + offset, player.position.y - offset), timeToComplete)
            .setEaseOutSine()
            .setOnComplete(() =>
            {
                OnPlayerMoved?.Invoke();
            });

        // Shuffle and Reset the deck;
        playingCardsDeck.ResetIndex();
        DeckShuffler.Shuffle(playingCardsDeck.Cards);

        vcam.LookAt = m_transform;
        vcam.Follow = m_transform;
        spriteRenderer.sprite = null;
    }

    private void HandleCombatEnd() => Destroy(gameObject);
}
