using UnityEngine;

public class ExitRoomDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite unlockedDoor = null;

    private bool isLocked = true;

    public bool IsLocked { get => isLocked; private set => isLocked = value; }

    private void OnEnable() => ExitRoomKey.OnKeyPickedUp += UnlockDoor;
    private void OnDisable() => ExitRoomKey.OnKeyPickedUp -= UnlockDoor;

    private void UnlockDoor()
    {
        IsLocked = false;
        spriteRenderer.sprite = unlockedDoor;
    }
}
