using UnityEngine;

public class ExitRoom : Room
{
    [Header("Exit Room References")]
    [SerializeField] private GameObject keyPrefab = null;

    protected override void AwakeHandler()
    {
        // TODO: Play a Boss Room sound

        var enemyRooms = GameObject.FindGameObjectsWithTag("Enemy Room");

        var keyRoom = enemyRooms[Random.Range(0, enemyRooms.Length)];
        Instantiate(keyPrefab, keyRoom.transform.position, Quaternion.identity);

        // TODO: Reset that Enemy Room
        // keyRoom.Reset();

        // TODO: Indicate the user that a key has spawned
    }
}
