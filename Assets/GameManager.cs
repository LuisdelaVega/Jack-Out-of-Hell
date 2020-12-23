using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private GameObject[] allRoomsExceptStartAndExit;
    [SerializeField] private GameObject exitRoom;
    [SerializeField] private Deck roomsDeck;

    [Header("Decks")]
    [SerializeField] private Deck enemyDeck;
    [SerializeField] private Deck playingCardDeck;

    #region Combat
    private Combatant player;
    private HealthBehaviour playerHealth;
    private int playerHandTotal;

    private Combatant enemy;
    private HealthBehaviour enemyHealth;
    #endregion

    public static event Action OnNewRoundStart;

    private void Awake()
    {
        RandomizeRooms();
        DeckShuffler.Shuffle(enemyDeck.Cards);
        enemyDeck.ResetIndex();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Combatant>();
        playerHealth = player.GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        PlayerCombat.OnHasDied += HandleGameOver;
        EnemySpawner.OnEnemySpawned += SetCurrentEnemy;
        Enemy.OnEnemyTurnEnd += HandleEnemyTurnEnd;
        PlayerCombat.OnPlayerBusted+= HandlePlayerBusted;
    }
    private void OnDisable()
    {
        PlayerCombat.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        PlayerCombat.OnHasDied -= HandleGameOver;
        EnemySpawner.OnEnemySpawned -= SetCurrentEnemy;
        Enemy.OnEnemyTurnEnd -= HandleEnemyTurnEnd;
        PlayerCombat.OnPlayerBusted -= HandlePlayerBusted;
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

    private void SetCurrentEnemy(Combatant enemy)
    {
        this.enemy = enemy;
        enemyHealth = enemy.GetComponent<HealthBehaviour>();
    }

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

        if (enemyHandTotal > 21)
            enemyHealth.DealDamage(1);
        else if (handDifference < 0)
            playerIsDead = playerHealth.DealDamage(1 + enemy.AttackModifier) == 0;
        else if (handDifference > 0)
            enemyIsDead = enemyHealth.DealDamage(1 + player.AttackModifier) == 0;

        if (!playerIsDead && !enemyIsDead)
            StartNewRound();
    }

    private void HandlePlayerBusted(int currentHealthForPlayer)
    {
        if (currentHealthForPlayer != 0)
            StartNewRound();
    }

    private void StartNewRound()
    {
        DeckShuffler.Shuffle(playingCardDeck.Cards);
        playingCardDeck.ResetIndex();
        OnNewRoundStart?.Invoke();
    }

    private void HandleGameOver()
    {
        // TODO: Implement Game Over
        Debug.Log("YOU LOSE!");
    }
}
