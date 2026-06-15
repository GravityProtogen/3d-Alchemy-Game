using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public CardType cardType;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardManager.Instance.SelectCard(cardType);
    }

    void Update()
    {
        if (CardManager.Instance.selectedCard == cardType)
            image.color = Color.yellow;
        else
            image.color = Color.white;
    }
}