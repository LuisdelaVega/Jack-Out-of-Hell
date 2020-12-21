using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IRoomTrigger
{
    [Header("References")]
    [SerializeField] private Deck enemyDeck = null;
    [SerializeField] protected Deck playingCardsDeck = null;

    [Header("Posiiton Offset"), Range(0f, 0.99f)]
    [SerializeField] private float offset = 0.75f;

    [Header("Time")]
    [SerializeField] private float timeToComplete = 1f;

    public static event Action<HealthBehaviour> OnEnemySpawned;
    public static event Action OnPlayerMoved;

    public void TriggerRoom(Transform player)
    {
        var enemy = Instantiate(enemyDeck.GetNextCard(), new Vector2(player.position.x - offset, player.position.y + offset), Quaternion.identity);
        
        if (enemy.TryGetComponent<HealthBehaviour>(out var healthBehaviour))
            OnEnemySpawned?.Invoke(healthBehaviour);

        // Move the player to the oposite corner of the room
        LeanTween.move(player.gameObject, new Vector2(player.position.x + offset, player.position.y - offset), timeToComplete)
            .setEaseOutSine()
            .setOnComplete(() =>
            {
                if (enemy.TryGetComponent<HealthBehaviour>(out var healthBehaviour))
                    OnPlayerMoved?.Invoke();
            });

        // Shuffle and Reset the deck;
        playingCardsDeck.ResetIndex();
        DeckShuffler.Shuffle(playingCardsDeck.Cards);

        Destroy(gameObject);
    }
}
