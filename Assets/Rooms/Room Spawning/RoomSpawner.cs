using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Deck roomsDeck;
    [SerializeField] private Button arrow;
    
    private Room parentRoom;
    private GameObject parentRoomSensor = null;
    private Transform m_transform = null;
    public static event Action<bool, GameObject> OnCollisionWithTheRoomSensorOfThePreviousRoom;
    public static event Action OnSpawnRoom;

    private bool isConnectedToAnotherRoom = false;

    private void Awake()
    {
        m_transform = transform;
        parentRoom = m_transform.parent.GetComponent<Room>();
        parentRoomSensor = parentRoom.GetComponentInChildren<RoomSensor>().gameObject;
    }

    private void OnEnable()
    {
        OnSpawnRoom += DeactivateButton;
        RoomPlacement.OnRoomPlaced += DestroyMe;
    }
    private void OnDisable()
    {
        OnSpawnRoom -= DeactivateButton;
        RoomPlacement.OnRoomPlaced -= DestroyMe;
    }

    public void SpawnRoom()
    {
        Instantiate(roomsDeck.GetNextCard(), m_transform.position, Quaternion.identity).GetComponent<Room>().RoomSensorOfThePreviousRoom = parentRoomSensor;
        OnSpawnRoom?.Invoke();
        Destroy(gameObject);
    }

    private void DeactivateButton() => SetButtonActive(false);
    public void SetButtonActive(bool value) => arrow.gameObject.SetActive(value);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentRoom.RoomSensorOfThePreviousRoom.Equals(collision.gameObject))
            OnCollisionWithTheRoomSensorOfThePreviousRoom?.Invoke(true, gameObject);
        else if (collision.CompareTag("Room Sensor"))
            isConnectedToAnotherRoom = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (parentRoom.RoomSensorOfThePreviousRoom.Equals(collision.gameObject))
            OnCollisionWithTheRoomSensorOfThePreviousRoom?.Invoke(false, gameObject);
        else if (collision.CompareTag("Room Sensor"))
            isConnectedToAnotherRoom = false;
    }

    private void DestroyMe()
    {
        if (isConnectedToAnotherRoom || roomsDeck.Cards.Length == roomsDeck.Index)
            Destroy(gameObject);
    }
}
