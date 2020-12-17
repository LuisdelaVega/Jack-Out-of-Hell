using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private GameObject[] allRoomsExceptStartAndExit;
    [SerializeField] private GameObject exitRoom;
    [SerializeField] private Deck roomsDeck;

    [Header("Enemies")]
    [SerializeField] private Deck enemyDeck;

    public static event Action OnCombatEnded;

    private void Awake()
    {
        RandomizeRooms();
        DeckShuffler.Shuffle(enemyDeck.Cards);
        enemyDeck.ResetIndex();
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        Enemy.OnEnemyTurnEnd += HandleEnemyTurnEnd;
    }
    private void OnDisable()
    {
        PlayerCombat.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        Enemy.OnEnemyTurnEnd -= HandleEnemyTurnEnd;
    }

    private void RandomizeRooms()
    {
        // Shuffle all the rooms
        DeckShuffler.Shuffle(allRoomsExceptStartAndExit);

        // Divide the rooms into two stacks
        GameObject[] firstHalfOfRooms = new GameObject[allRoomsExceptStartAndExit.Length / 2];
        GameObject[] secondHalfOfRooms = new GameObject[allRoomsExceptStartAndExit.Length / 2 + 1];

        for (int i = 0; i < firstHalfOfRooms.Length; i++)
        {
            firstHalfOfRooms[i] = allRoomsExceptStartAndExit[i];
            secondHalfOfRooms[i] = allRoomsExceptStartAndExit[firstHalfOfRooms.Length + i];
        }

        // Add the Exit Room to the second stack
        secondHalfOfRooms[secondHalfOfRooms.Length - 1] = exitRoom;
        // Then shuffle the second stack
        DeckShuffler.Shuffle(secondHalfOfRooms);

        // Combine both stacks into a Room Deck
        GameObject[] allRooms = new GameObject[allRoomsExceptStartAndExit.Length + 1];

        firstHalfOfRooms.CopyTo(allRooms, 0);
        secondHalfOfRooms.CopyTo(allRooms, firstHalfOfRooms.Length);

        roomsDeck.SetCards(allRooms);
    }

    private void HandlePlayerTurnEnd(int handTotal)
    {
        Debug.Log($"Player's hand total: {handTotal}");
    }

    private void HandleEnemyTurnEnd(int handTotal)
    {
        Debug.Log($"Enemy's hand total: {handTotal}");

        // TODO: This won't really happen like this. We need to trigger a new round if both the enemy and the player are alive
        OnCombatEnded?.Invoke();
    }
}
