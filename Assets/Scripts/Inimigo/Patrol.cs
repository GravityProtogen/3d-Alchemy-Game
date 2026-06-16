using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;

    public float rotationSpeed = 180f;
    public float turnThreshold = 5f;
    public float moveSpeed = 3f;
    public float waitTime = 2f;

    bool distracted = false;
    Vector3 distractionPoint;
    int currentPoint = 0;
    int direction = 1;

    bool waiting = false;

    void Update()
    {
        if (distracted)
        {
            LookAtDistraction();
            return;
        }
        if (patrolPoints.Length == 0 || waiting)
            return;

        Transform target = patrolPoints[currentPoint];

        // Ignore Y
        Vector3 targetPosition = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Horizontal rotation only
        Vector3 lookDirection = targetPosition - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );

                    float angle = Quaternion.Angle(
                        transform.rotation,
                        targetRotation
                    );

                    // Only move when mostly facing target
                    if (angle > turnThreshold)
                        return;
                }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            bool atEndPoint =
                currentPoint == 0 ||
                currentPoint == patrolPoints.Length - 1;

            if (atEndPoint)
            {
                StartCoroutine(WaitAtPoint());
            }
            else
            {
                currentPoint += direction;
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        waiting = true;

        yield return new WaitForSeconds(waitTime);

        if (currentPoint == patrolPoints.Length - 1)
            direction = -1;

        else if (currentPoint == 0)
            direction = 1;

        currentPoint += direction;

        waiting = false;
    }
    public void Distract(Vector3 point, float duration)
    {
        Debug.Log(name + " distracted");
        StartCoroutine(DistractRoutine(point, duration));
    }
    IEnumerator DistractRoutine(Vector3 point, float duration)
    {
        distracted = true;

        distractionPoint = point;

        yield return new WaitForSeconds(duration);

        distracted = false;
    }
    void LookAtDistraction()
    {
        Vector3 direction =
            distractionPoint - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}