using UnityEngine;
using UnityEngine.Splines;

public class TrainController : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDuration = 5f;

    [Header("Train Coaches")]
    [SerializeField] private Transform engine;
    [SerializeField] private Transform[] coaches;
    [SerializeField] private float coachSpacing = 2f;

    private float splinePosition = 0f;
    private bool isStopping = false;

    private void Update()
    {
        if (isStopping) return;

        // Move the train along the spline
        splinePosition += (speed * Time.deltaTime) / spline.CalculateLength();
        if (splinePosition >= 1f)
        {
            // Despawn the train when it reaches the end
            DespawnTrain();
            return;
        }

        // Update engine and coaches positions
        UpdateTrainPosition();
    }

    private void UpdateTrainPosition()
    {
        // Move the engine along the spline
        Vector3 enginePosition = spline.EvaluatePosition(splinePosition);
        engine.position = enginePosition;

        // Update coaches based on the engine position
        float coachPosition = splinePosition;
        for (int i = 0; i < coaches.Length; i++)
        {
            coachPosition -= coachSpacing / spline.CalculateLength();
            coachPosition = Mathf.Max(0f, coachPosition);

            Vector3 coachPositionOnSpline = spline.EvaluatePosition(coachPosition);
            coaches[i].position = coachPositionOnSpline;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the train has entered the station trigger
        if (other.CompareTag("Station"))
        {
            StartCoroutine(StopAtStation());
        }
    }

    private System.Collections.IEnumerator StopAtStation()
    {
        isStopping = true;
        yield return new WaitForSeconds(stopDuration);
        isStopping = false;
    }

    private void DespawnTrain()
    {
        Destroy(gameObject);
    }
}
