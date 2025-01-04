using UnityEngine;

public class TramSpawner : MonoBehaviour
{
    [Header("Tram Settings")]
    [SerializeField] private GameObject tramPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnTrams());
    }

    private System.Collections.IEnumerator SpawnTrams()
    {
        while (true)
        {
            SpawnTram();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnTram()
    {
        Instantiate(tramPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
