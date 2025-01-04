using System.Collections;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] vehiclePrefabs;
    [SerializeField] private Transform[][] roadLanes;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRange = 20f;

    private void Start()
    {
        StartCoroutine(SpawnVehicles());
    }

    private IEnumerator SpawnVehicles()
    {
        while (true)
        {
            SpawnVehicle();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnVehicle()
    {
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        Vector3 spawnPosition = GetRandomSpawnPosition();

        Quaternion spawnRotation = Quaternion.identity;
        GameObject vehicle = Instantiate(vehiclePrefab, spawnPosition, spawnRotation);
        
        int laneIndex = Random.Range(0, roadLanes.Length);
        
        Transform[] selectedPath = roadLanes[laneIndex];
        vehicle.GetComponent<VehicleController>().SetPath(selectedPath);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle * spawnRange;
        return spawnPoints[0].position + new Vector3(randomDirection.x, 0f, randomDirection.y);
    }
}
