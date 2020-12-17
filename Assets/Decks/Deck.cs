using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    [SerializeField] private GameObject[] cards;
    private int index = 0;

    public GameObject[] Cards { get => cards; private set { cards = value; } }
    public int Index { get => index; private set { index = value; } }

    public void SetCards(GameObject[] cards)
    {
        Cards = cards;
        Index = 0;
    }

    public GameObject GetNextCard() => Cards[Index++];
    public void ResetIndex() => Index = 0;
}
