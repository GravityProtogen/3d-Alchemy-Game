using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public CardType selectedCard = CardType.None;

    void Awake()
    {
        Instance = this;
    }

    public void SelectCard(CardType card)
    {
        selectedCard = card;

        Debug.Log("Selected: " + card);
    }
}