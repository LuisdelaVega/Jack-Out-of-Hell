using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button placeButton = null;
    [SerializeField] private Button rotateButton = null;

    private RoomSensor roomSensor;
    private GameObject roomSpawner;

    private Transform parentTransform = null;
    private bool roomIsConnected = false;

    public static event Action OnRoomPlaced;

    private void Awake()
    {
        parentTransform = transform.parent;
        roomSensor = parentTransform.GetComponentInChildren<RoomSensor>();
    }

    private void OnEnable() => RoomSpawner.OnCollisionWithTheRoomSensorOfThePreviousRoom += IsTheRoomConnected;
    private void OnDisable() => RoomSpawner.OnCollisionWithTheRoomSensorOfThePreviousRoom -= IsTheRoomConnected;
    private void Start() => placeButton.interactable = roomIsConnected;

    public void RotateRoom()
    {
        rotateButton.interactable = false;
        Vector3 rotation = parentTransform.rotation.eulerAngles;
        rotation.z -= 90;

        LeanTween.rotate(parentTransform.gameObject, rotation, 0.25f).setEaseOutElastic().setOnComplete(() => rotateButton.interactable = true);
    }

    private void IsTheRoomConnected(bool value, GameObject spawner)
    {
        roomIsConnected = value;
        placeButton.interactable = roomIsConnected;

        roomSpawner = spawner;
    }

    public void PlaceRoom()
    {
        if (!roomIsConnected) return;

        Vector2 destination = new Vector2(roomSensor.transform.position.x, roomSensor.transform.position.y);

        Destroy(gameObject);
        Destroy(roomSpawner);

        OnRoomPlaced?.Invoke();
    }
}
