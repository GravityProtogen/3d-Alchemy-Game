using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject placeholder;
    int originalSiblingIndex;
    
    public CardType cardType;

    RectTransform rectTransform;
    Vector2 originalPosition;

    Transform originalParent;
    Canvas canvas;

    CanvasGroup canvasGroup;

    Image image;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardManager.Instance.SelectCard(cardType);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        originalSiblingIndex =
            transform.GetSiblingIndex();

        // Create placeholder
        placeholder = new GameObject("Placeholder");

        LayoutElement placeholderLayout =
            placeholder.AddComponent<LayoutElement>();

        LayoutElement myLayout =
            GetComponent<LayoutElement>();

        if (myLayout != null)
        {
            placeholderLayout.preferredWidth =
                myLayout.preferredWidth;

            placeholderLayout.preferredHeight =
                myLayout.preferredHeight;

            placeholderLayout.flexibleWidth =
                myLayout.flexibleWidth;

            placeholderLayout.flexibleHeight =
                myLayout.flexibleHeight;
        }
        else
        {
            placeholderLayout.preferredWidth =
                rectTransform.rect.width;

            placeholderLayout.preferredHeight =
                rectTransform.rect.height;
        }

        placeholder.transform.SetParent(
            originalParent,
            false
        );

        placeholder.transform.SetSiblingIndex(
            originalSiblingIndex
        );

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform);

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent);

        transform.SetSiblingIndex(
            originalSiblingIndex
        );
        CardUI targetCard = null;

        if (eventData.pointerEnter != null)
        {
            targetCard =
                eventData.pointerEnter.GetComponentInParent<CardUI>();
        }
        if (placeholder != null)
        {
            Destroy(placeholder);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            originalParent.GetComponent<RectTransform>()
        );
        if (targetCard != null && targetCard != this)
        {
            CraftManager.Instance.TryCraft(
                cardType,
                targetCard.cardType
            );

            Debug.Log(
                cardType + " dropped on " +
                targetCard.cardType
            );
        }
    }

    void Update()
    {
        if (CardManager.Instance.selectedCard == cardType)
            image.color = Color.yellow;
        else
            image.color = Color.white;
    }
}