using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private GameObject[] allRoomsExceptStartAndExit;
    [SerializeField] private GameObject exitRoom;
    [SerializeField] private Deck roomsDeck;

    [Header("Enemies")]
    [SerializeField] private Deck enemyDeck;

    private void Awake()
    {
        RandomizeRooms();
        DeckShuffler.Shuffle(enemyDeck.Cards);
        enemyDeck.ResetIndex();
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
}
