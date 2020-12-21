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

    #region Combat
    private HealthBehaviour playerHealth;
    private int playerHandTotal;

    private HealthBehaviour enemyHealth;
    #endregion

    public static event Action OnRoundEnded;

    private void Awake()
    {
        RandomizeRooms();
        DeckShuffler.Shuffle(enemyDeck.Cards);
        enemyDeck.ResetIndex();

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        PlayerCombat.OnHasDied += HandleGameOver;
        EnemySpawner.OnEnemySpawned += SetCurrentEnemy;
        Enemy.OnEnemyTurnEnd += HandleEnemyTurnEnd;
    }
    private void OnDisable()
    {
        PlayerCombat.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        PlayerCombat.OnHasDied -= HandleGameOver;
        EnemySpawner.OnEnemySpawned-= SetCurrentEnemy;
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

    private void SetCurrentEnemy(HealthBehaviour enemyHealth) => this.enemyHealth = enemyHealth;

    private void HandlePlayerTurnEnd(int handTotal)
    {
        Debug.Log($"Player's hand total: {handTotal}");
        playerHandTotal = handTotal;
    }

    private void HandleEnemyTurnEnd(int enemyHandTotal)
    {
        Debug.Log($"Enemy's hand total: {enemyHandTotal}");

        var handDifference = playerHandTotal - enemyHandTotal;
        Debug.Log($"Hand difference {handDifference}");

        bool playerIsDead = false;
        bool enemyIsDead = false;

        if (handDifference < 0)
            playerIsDead = playerHealth.DealDamage(-handDifference) == 0;
        else
            enemyIsDead = enemyHealth.DealDamage(handDifference) == 0;

        if (!playerIsDead && !enemyIsDead)
            OnRoundEnded?.Invoke();
    }

    private void HandleGameOver()
    {
        // TODO: Implement Game Over
        Debug.Log("YOU LOSE!");
    }
}
