using UnityEngine;

public class OxygenGust : MonoBehaviour
{
    public float radius = 4f;

    void Start()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                radius
            );

        foreach (Collider hit in hits)
        {
            EnemyPatrol patrol =
                hit.GetComponentInParent<EnemyPatrol>();

            if (patrol != null)
            {
                patrol.Distract(
                    transform.position,
                    5f
                );
            }
        }
        Destroy(gameObject, 1f);
    }
}