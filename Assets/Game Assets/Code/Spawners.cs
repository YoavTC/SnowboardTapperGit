using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    [SerializeField] private AnimationCurve movementSpeedCurve;
    [SerializeField] public float obstaclesSpeed;
    
    public float cuttingPoint { get; private set; }
    void Start()
    {
        Vector3 worldBottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        cuttingPoint = worldBottomLeft.y - 2f;

        StartCoroutine(SpawnObstacle());
        InvokeRepeating(nameof(UpgradeMovementSpeed), 2f, 5f);
    }
    
    
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private List<Transform> obstacleSpawners = new List<Transform>();

    private Transform lastSpawner;

    IEnumerator SpawnObstacle()
    {
        Transform randomSpawnPoint = HelperFunctions.GetRandomObject(obstacleSpawners, lastSpawner);
        
        Transform spawnedObstacle = Instantiate(obstaclePrefab, randomSpawnPoint.position, Quaternion.identity);
        spawnedObstacle.GetComponent<Obstacle>().spawners = this;
        spawnedObstacle.SetParent(transform);

        lastSpawner = randomSpawnPoint;
        
        yield return new WaitForSeconds(1f - (movementSpeedCurve.Evaluate(movementSpeedCurveProgress) * 0.06f));
        StartCoroutine(SpawnObstacle());
    }

    [SerializeField] private int movementSpeedCurveProgress = 0;
    [SerializeField] private Movement movementScript;
    
    void UpgradeMovementSpeed()
    {
        movementSpeedCurveProgress++;
        obstaclesSpeed = movementSpeedCurve.Evaluate(movementSpeedCurveProgress);

        Transform[] children = HelperFunctions.GetChildrenWithTag(transform, "Respawn").ToArray();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].GetComponent<Obstacle>().SetSpeed(obstaclesSpeed);
        }

        movementScript.speedMultiplier += 0.09f;
    }

    public void ResetGame()
    {
        Transform[] children = HelperFunctions.GetChildrenWithTag(transform, "Respawn").ToArray();

        for (int i = 0; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        movementSpeedCurveProgress = 0;
        obstaclesSpeed = 3f;
        movementScript.ResetGame();
    }
}
