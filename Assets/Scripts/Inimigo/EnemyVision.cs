using UnityEngine;
using TMPro;

public class EnemyVision : MonoBehaviour
{
    bool stunned = false;
    Vector3 originalScale;
    public TMP_Text alertText;
    bool playerAlreadyDetected = false;
    [Header("Detection")]
    public float detectionTime = 1.5f;
    float currentDetection = 0f;
    [Header("Vision")]
    public float viewDistance = 8f;
    [Range(0, 180)]
    public float viewAngle = 60f;

    [Header("References")]
    public Transform player;

    [Header("Layers")]
    public LayerMask obstacleMask;

    public void Stun(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StunRoutine(duration));
    }
    System.Collections.IEnumerator StunRoutine(float duration)
    {
        stunned = true;

        HideIndicator();

        yield return new WaitForSeconds(duration);

        stunned = false;
    }
    void Update()
        {
            if (stunned)
            return;
            bool canSeePlayer = DetectPlayer();

            if (canSeePlayer)
            {
                currentDetection += Time.deltaTime;

                if (!playerAlreadyDetected)
                {
                    ShowQuestionMark();
                    UpdateIndicator();
                }

                if (currentDetection >= detectionTime && !playerAlreadyDetected)
                {
                    playerAlreadyDetected = true;
                    PlayerDetected();
                }
            }
            else
            {
                currentDetection -= Time.deltaTime;

                if (currentDetection < 0)
                    currentDetection = 0;

                if (currentDetection == 0)
                {
                    HideIndicator();
                }
            }
        }

    bool DetectPlayer()
    {
        if (player == null)
            return false;

        Vector3 directionToPlayer =
            player.position - transform.position;

        directionToPlayer.y = 0;

        float distanceToPlayer =
            directionToPlayer.magnitude;

        // Too far away
        if (distanceToPlayer > viewDistance)
            return false;

        float angle =
            Vector3.Angle(
                transform.forward,
                directionToPlayer
            );

        // Outside cone
        if (angle > viewAngle / 2)
            return false;

        // Check walls
        Vector3 rayOrigin =
            transform.position + Vector3.up * 1.5f;

        Vector3 rayDirection =
            (player.position - rayOrigin).normalized;

        if (Physics.Raycast(
            rayOrigin,
            rayDirection,
            out RaycastHit hit,
            viewDistance,
            obstacleMask | (1 << player.gameObject.layer)
        ))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }

            return false;
        }
        return false;
    }

    void PlayerDetected()
    {
        ShowExclamationMark();

        Debug.Log("PLAYER DETECTED!");

    }
    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;

        Gizmos.color = Color.red;

        Vector3 leftBoundary =
            Quaternion.Euler(0, -viewAngle / 2, 0) *
            transform.forward;

        Vector3 rightBoundary =
            Quaternion.Euler(0, viewAngle / 2, 0) *
            transform.forward;

        Gizmos.DrawRay(origin, leftBoundary * viewDistance);
        Gizmos.DrawRay(origin, rightBoundary * viewDistance);
        Gizmos.DrawRay(origin, transform.forward * viewDistance);

        // Draw line between the ends of the cone
        Gizmos.DrawLine(
            origin + leftBoundary * viewDistance,
            origin + rightBoundary * viewDistance
        );
    }
    void ShowQuestionMark()
    {
        alertText.gameObject.SetActive(true);
        alertText.text = "?";
    }

    void ShowExclamationMark()
    {
        alertText.gameObject.SetActive(true);
        alertText.text = "!";

        alertText.color = Color.red;
        alertText.transform.localScale =
            originalScale * 2f;
    }

    void HideIndicator()
    {
        alertText.gameObject.SetActive(false);

        alertText.transform.localScale = originalScale;
        alertText.color = Color.white;
    }
    //scale with detection
    void Start()
    {
        originalScale = alertText.transform.localScale;
    }
    void UpdateIndicator()
    {
        float progress = currentDetection / detectionTime;

        // Scale from 50% to 150%
        float scaleMultiplier = Mathf.Lerp(0.5f, 1.5f, progress);

        alertText.transform.localScale =
            originalScale * scaleMultiplier;

        // White -> Yellow -> Red
        Color indicatorColor = Color.Lerp(
            Color.white,
            Color.red,
            progress
        );

        alertText.color = indicatorColor;
    }
    void LateUpdate()
    {
        if (alertText != null && Camera.main != null)
        {
            alertText.transform.forward =
                Camera.main.transform.forward;
        }
    }
}