using UnityEngine;

public class CardAreaAnimator : MonoBehaviour
{
    public RectTransform cardArea;

    public float hiddenY = -120f;
    public float shownY = 0f;

    public float speed = 8f;

    bool hovering;

    // CAMERA
    public Transform cameraTarget;

    // These are FULL LOCAL POSITIONS
    public Vector3 cameraOffsetHidden;
    public Vector3 cameraOffsetShown;

    void Update()
    {
        // UI ANIMATION
        float target = hovering ? shownY : hiddenY;

        Vector2 pos = cardArea.anchoredPosition;

        pos.y = Mathf.Lerp(
            pos.y,
            target,
            Time.deltaTime * speed
        );

        cardArea.anchoredPosition = pos;

        // CAMERA ANIMATION
        Vector3 targetOffset =
            hovering ? cameraOffsetShown : cameraOffsetHidden;

        cameraTarget.localPosition = Vector3.Lerp(
            cameraTarget.localPosition,
            targetOffset,
            Time.deltaTime * speed
        );
    }

    public void SetHovering(bool value)
    {
        hovering = value;
    }
}