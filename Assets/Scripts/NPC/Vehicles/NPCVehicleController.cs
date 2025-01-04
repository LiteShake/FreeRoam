using System.Collections;
using UnityEngine;

public class NPCVehicleController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waypointTolerance = 1f;
    [SerializeField] private float outOfBoundsValue;

    private Transform[] path;
    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (path != null && path.Length > 0)
        {
            MoveAlongPath();
        }
    }

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
        currentWaypointIndex = 0;
    }

    private void MoveAlongPath()
    {
        if (currentWaypointIndex >= path.Length) return;

        Transform targetWaypoint = path[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.y = 0f;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointTolerance)
        {
            currentWaypointIndex++;
        }

        if (transform.position.magnitude > outOfBoundsValue)
        {
            DespawnVehicle();
        }
    }

    private void DespawnVehicle()
    {
        Destroy(gameObject);
    }
}
