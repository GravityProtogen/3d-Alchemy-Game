using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;

    public float moveSpeed = 3f;
    public float waitTime = 2f;

    int currentPoint = 0;
    int direction = 1;

    bool waiting = false;

    void Update()
    {
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
            transform.rotation = Quaternion.LookRotation(lookDirection);

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
}