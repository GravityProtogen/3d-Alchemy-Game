using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            smoothSpeed * Time.deltaTime
        );
    }
}