using UnityEngine;

public enum ConnectionDirection
{
    Norte,
    Leste,
    Oeste,
    Sul
}

public class ConnectionPoint : MonoBehaviour
{
    public ConnectionDirection direction;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}