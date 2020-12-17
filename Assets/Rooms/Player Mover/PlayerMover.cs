using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button arrow = null;

    private Vector2 destination;
    private bool pathIsLocked = false;

    public bool hasAPathThrough = false;
    public static event Action<Vector2> OnMovePlayer;

    private void OnEnable()
    {
        RoomSpawner.OnSpawnRoom += DeactivateButton;
        ExitRoomKey.OnKeyPickedUp += UnlockPath;
    }
    private void OnDisable()
    {
        RoomSpawner.OnSpawnRoom -= DeactivateButton;
        ExitRoomKey.OnKeyPickedUp -= UnlockPath;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMover>(out var otherPlayerMover))
        {
            otherPlayerMover.hasAPathThrough = hasAPathThrough = true;

            /**
             * Store a reference for the Room Sensor of the connecting Room.
             * The position of this Room Sensor will be the destination for when moving the player.
             */
            destination = otherPlayerMover.transform.parent.GetComponentInChildren<RoomSensor>().transform.position;
        }
        else if (collision.TryGetComponent<ExitRoomDoor>(out var exitRoomDoor))
            pathIsLocked = exitRoomDoor.IsLocked;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMover>(out var otherPlayerMover))
            otherPlayerMover.hasAPathThrough = hasAPathThrough = false;
        else if (collision.TryGetComponent<ExitRoomDoor>(out _))
            pathIsLocked = false;
    }

    private void DeactivateButton() => SetButtonActive(false);
    public void SetButtonActive(bool value)
    {
        if (hasAPathThrough)
            arrow.gameObject.SetActive(value);
        else
            arrow.gameObject.SetActive(false);
    }

    private void UnlockPath() => pathIsLocked = false;

    public void MovePlayer()
    {
        if (!pathIsLocked)
            OnMovePlayer?.Invoke(destination);
        else
        {
            // TODO: Let the Player know the path is locked. Play some locked door sound and have some animation happen.
        }
    }
}
