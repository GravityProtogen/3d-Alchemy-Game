using UnityEngine;

public class HydrogenExplosion : MonoBehaviour
{
    public float radius = 6f;
    public float stunDuration = 4f;
    public Transform visual;

    void Start()
    {
        visual.localScale =
            Vector3.one * radius * 2f;
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                radius
            );
        Debug.Log("Hits: " + hits.Length);

        foreach (Collider c in hits)
        {
            Debug.Log(c.name);
        }

        foreach (Collider hit in hits)
        {
            EnemyVision vision =
                hit.GetComponentInParent<EnemyVision>();

            if (vision != null)
            {
                vision.Stun(stunDuration);
            }
        }

        Destroy(gameObject, 1f);
    }
}