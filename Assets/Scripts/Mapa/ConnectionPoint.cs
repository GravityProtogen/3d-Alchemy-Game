using UnityEngine;

public enum ConnectionDirection
{
    Norte,
    Sul,
    Leste,
    Oeste
}

public class ConnectionPoint : MonoBehaviour
{
    public ConnectionDirection direction;

    private void OnValidate()
    {
        Debug.Log(
            gameObject.name +
            " Direction = " +
            direction
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(
            transform.position,
            transform.forward * 2f
        );
    }
}