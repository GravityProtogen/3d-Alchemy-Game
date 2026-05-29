using UnityEngine;
using UnityEngine.EventSystems;

public class HoverZone : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public CardAreaAnimator animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetHovering(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetHovering(false);
    }
}