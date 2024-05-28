using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;

    int currentWayPointIndex = 0;

    [SerializeField] float speed = 1f;

    bool isWaiting;

    [SerializeField] float platformWaitAtWaypoint = 1f;

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, waypoints[currentWayPointIndex].transform.position) <= 0.1f && !isWaiting)
        {
            StartCoroutine(WaitAtWaypoint());
        }
        if(Vector3.Distance(transform.position, waypoints[currentWayPointIndex].transform.position) > 0.1 && isWaiting)
        {
            isWaiting = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, speed * Time.deltaTime);
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(platformWaitAtWaypoint);

        currentWayPointIndex++;

        if(currentWayPointIndex >= waypoints.Length)
        {
            currentWayPointIndex = 0;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

}
