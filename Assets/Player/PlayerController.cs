using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;

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
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        // Rotação
        if (movement != Vector3.zero)
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
}