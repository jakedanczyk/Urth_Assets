using UnityEngine;
using System.Collections;

public class RandomWaypoint : MonoBehaviour
{
    Vector3 targetPos;
    float MovementSpeed = 5.0f;
    float CloseEnough = 0.5f;

    IEnumerator Start()
    {
        targetPos = transform.position;
        while (true)
        {
            // do not move
            yield return new WaitForSeconds(2.0f);
            // choose new location
            targetPos = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5));
            // wait until we arrive
            while ((transform.position - targetPos).magnitude > CloseEnough)
            {
                yield return new WaitForSeconds(0.0f);
            }
        }
    }

    void Update()
    {
        // have we arrived?
        if ((transform.position - targetPos).magnitude > CloseEnough)
        {
            // nope, turn to and go
            transform.LookAt(targetPos);
            transform.position += (targetPos - transform.position).normalized *
                MovementSpeed * Time.deltaTime;
        }
    }
}

