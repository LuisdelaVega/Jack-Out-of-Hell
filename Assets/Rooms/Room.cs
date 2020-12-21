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
    private Tilemap ground;

    private void Awake()
    {
        ground = GetComponentInChildren<Tilemap>();
        ground.color = roomProperties.groundColor;

        if (roomIconPrefab != null)
            roomIcon = Instantiate(roomIconPrefab, transform.position, Quaternion.identity);

        AwakeHandler();
    }

    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawned += HandleCombatStart;
        Enemy.OnHasDied += HandleCombatEnd;
        PlayerCombat.OnHasDied += HandleCombatEnd;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawned -= HandleCombatStart;
        Enemy.OnHasDied -= HandleCombatEnd;
        PlayerCombat.OnHasDied -= HandleCombatEnd;
    }

    private void HandleCombatStart<T>(T _) => LeanTween.alpha(ground.gameObject, 0.4f, 0.5f);// new Color(ground.color.r, ground.color.g, ground.color.b, 0.4f);
    private void HandleCombatEnd() => LeanTween.alpha(ground.gameObject, 1, 0.5f); // ground.color = new Color(ground.color.r, ground.color.g, ground.color.b, 1f);

    protected abstract void AwakeHandler();
}
