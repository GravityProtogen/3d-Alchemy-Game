using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Aiming
    public float speed;
    public Transform aimTarget;
    public LineRenderer aimLine;
    public float cardRange = 5f;
    public LayerMask obstacleLayers;
    public Transform aoePreview;
    public float oxygenRadius = 2f;

    private Vector2 move;
    private Animator animator;

    private float currentSneakValue;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();

        HandleAiming();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        // Rotação
        bool cardSelected =
            CardManager.Instance.selectedCard != CardType.None;

        if (!cardSelected && movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movement),
                0.15f
            );
        }

        // Movimento
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Valor alvo do Animator
        float targetValue = movement.magnitude > 0 ? 1f : 0f;

        // Suaviza transição (passa pelo 0.5 naturalmente)
        currentSneakValue = Mathf.Lerp(
            currentSneakValue,
            targetValue,
            Time.deltaTime * 5f
        );

        animator.SetFloat("SneakingSpeed", currentSneakValue);
    }
    void HandleAiming()
    {
        bool cardSelected =
            CardManager.Instance.selectedCard != CardType.None;


        if (aimLine != null)
        {
            aimLine.enabled = cardSelected;
        }
        if (aoePreview != null)
        {
            aoePreview.gameObject.SetActive(cardSelected);
        }

        if (!cardSelected)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 direction = hitPoint - transform.position;
            direction.y = 0;

            float targetDistance =
                Mathf.Min(direction.magnitude, cardRange);

            direction = direction.normalized;

            Vector3 finalTarget =
                transform.position + direction * targetDistance;
            Vector3 rayOrigin =
            transform.position + Vector3.up * 0.5f;

            if (Physics.Raycast(
                rayOrigin,
                direction,
                out RaycastHit hit,
                targetDistance,
                obstacleLayers))
            {
                Debug.Log("HIT WALL: " + hit.collider.name);
                finalTarget = hit.point - direction * 0.2f;
            }
            aimTarget.position = finalTarget;
            aoePreview.position =
            finalTarget + Vector3.up * 0.02f;

            aoePreview.localScale =
                new Vector3(
                    oxygenRadius * 2f,
                    0.01f,
                    oxygenRadius * 2f
                );
            
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            DrawAimArc();
            Debug.DrawRay(
            rayOrigin,
            direction * targetDistance,
            Color.red
            );
        }
    }
    void DrawAimArc()
    {
        int points = 20;

        aimLine.positionCount = points;

        Vector3 start = transform.position;
        Vector3 end = aimTarget.position;

        float arcHeight = 1.5f;

        for (int i = 0; i < points; i++)
        {
            float t = i / (float)(points - 1);

            Vector3 point = Vector3.Lerp(start, end, t);

            point.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            aimLine.SetPosition(i, point);
        }
    }
}