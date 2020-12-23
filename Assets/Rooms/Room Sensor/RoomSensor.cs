using UnityEngine;

public class RoomSensor : MonoBehaviour
{
    private RoomSpawner[] roomSpawners;
    private PlayerMover[] playerMovers;

    private Transform m_transform = null;
    private bool playerIsInTheRoom = false;
    private bool thereIsATriggerInTheRoom = false;

    private void Awake()
    {
        m_transform = transform;
        roomSpawners = m_transform.parent.GetComponentsInChildren<RoomSpawner>();
        playerMovers = m_transform.parent.GetComponentsInChildren<PlayerMover>();

        SetButtonsActive(false);
    }

    private void OnEnable()
    {
        //EnemySpawner.OnEnemySpawned += CombatHasStarted;
        RoomPlacement.OnRoomPlaced += OnRoomPlacedHandler;
        Enemy.OnHasDied += CombatHasEnded;
    }


    private void OnDisable()
    {
        //EnemySpawner.OnEnemySpawned -= CombatHasStarted;
        RoomPlacement.OnRoomPlaced -= OnRoomPlacedHandler;
        Enemy.OnHasDied -= CombatHasEnded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInTheRoom = true;
            SetButtonsActive(true && !thereIsATriggerInTheRoom);
        }
        else if (collision.TryGetComponent<IRoomTrigger>(out var _))
        {
            thereIsATriggerInTheRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInTheRoom = false;
            SetButtonsActive(false);
        }
        else if (collision.TryGetComponent<IRoomTrigger>(out var _))
        {
            thereIsATriggerInTheRoom = false;
        }
    }

    private void OnRoomPlacedHandler() => SetButtonsActive(playerIsInTheRoom);

    private void SetButtonsActive(bool value)
    {
        for (int i = 0; i < roomSpawners.Length; i++)
        {
            var roomSpawner = roomSpawners[i];
            var playerMover = playerMovers[i];

            if (!value)
            {
                if (roomSpawner != null)
                    roomSpawner.SetButtonActive(value);
                playerMover.SetButtonActive(value);
                continue;
            }

            if (roomSpawner != null)
                roomSpawner.SetButtonActive(value);
            else
                playerMover.SetButtonActive(value);
        }
    }

    //private void CombatHasStarted<T>(T _) => isAnEnemyRoom = true;
    private void CombatHasEnded()
    {
        if (playerIsInTheRoom)
            thereIsATriggerInTheRoom = false;
    }
}
