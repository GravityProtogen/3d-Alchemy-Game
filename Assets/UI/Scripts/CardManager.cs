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
        if (selectedCard == card)
        {
            selectedCard = CardType.None;
            return;
        }

        selectedCard = card;
    }
    public void ClearSelection()
    {
        selectedCard = CardType.None;
    }
}