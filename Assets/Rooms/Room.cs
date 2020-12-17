using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Room : MonoBehaviour
{
    public GameObject RoomSensorOfThePreviousRoom { get; set; }

    [Header("References")]
    [SerializeField] protected GameObject roomIconPrefab = null;

    [Header("Room Properties")]
    [SerializeField] private RoomProperties roomProperties = null;

    protected GameObject roomIcon = null;

    private void Awake()
    {
        var ground = GetComponentInChildren<Tilemap>();
        ground.color = roomProperties.groundColor;

        if (roomIconPrefab != null)
            roomIcon = Instantiate(roomIconPrefab, transform.position, Quaternion.identity);

        AwakeHandler();
    }

    protected abstract void AwakeHandler();
}
