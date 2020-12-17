using UnityEngine;

public class RoomSensor : MonoBehaviour
{
    private RoomSpawner[] roomSpawners;
    private PlayerMover[] playerMovers;

    private Transform m_transform = null;
    private bool playerIsInTheRoom = false;

    private void Awake()
    {
        m_transform = transform;
        roomSpawners = m_transform.parent.GetComponentsInChildren<RoomSpawner>();
        playerMovers = m_transform.parent.GetComponentsInChildren<PlayerMover>();

        SetButtonsActive(false);
    }

    private void OnEnable() => RoomPlacement.OnRoomPlaced += OnRoomPlacedHandler;
    private void OnDisable() => RoomPlacement.OnRoomPlaced -= OnRoomPlacedHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInTheRoom = true;
            SetButtonsActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsInTheRoom = false;
            SetButtonsActive(false);
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
}
