using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IRoomTrigger
{
    [Header("References")]
    [SerializeField] private Deck enemyDeck = null;
    [SerializeField] protected Deck playingCardsDeck = null;

    [Header("Posiiton Offset")]
    [SerializeField] private float offset = 0.75f;

    [Header("Time")]
    [SerializeField] private float timeToComplete = 1f;

    public static event Action OnEnemySpawned;

    public void TriggerRoom(Transform player)
    {
        Instantiate(enemyDeck.GetNextCard(), new Vector2(player.position.x - offset, player.position.y + offset), Quaternion.identity);
        LeanTween.move(player.gameObject, new Vector2(player.position.x + offset, player.position.y - offset), timeToComplete)
            .setEaseOutSine()
            .setOnComplete(() => OnEnemySpawned?.Invoke());

        // Shuffle and Reset the deck;
        playingCardsDeck.ResetIndex();
        DeckShuffler.Shuffle(playingCardsDeck.Cards);

        Destroy(gameObject);
    }
}
