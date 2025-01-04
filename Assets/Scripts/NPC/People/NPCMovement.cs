using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waypointTolerance = 1f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Animator animator;

    private int currentWaypointIndex = 0;
    private bool isMoving = true;

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveAlongWaypoints();
        }
    }

    private void MoveAlongWaypoints()
    {
        if (waypoints.Count == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.y = 0f;

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction.magnitude > waypointTolerance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }

        if (direction.magnitude > waypointTolerance)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
