using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    [Header("Train Settings")]
    [SerializeField] private GameObject trainPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnTrains());
    }

    private System.Collections.IEnumerator SpawnTrains()
    {
        while (true)
        {
            SpawnTrain();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnTrain()
    {
        Instantiate(trainPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
