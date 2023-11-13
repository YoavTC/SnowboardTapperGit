using Unity.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [HideInInspector]
    public Spawners spawners;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer currentSprite;
    [SerializeField] [ReadOnly] private float obstacleSpeed;
    private float cuttingPoint;

    public void SetSpeed(float speed)
    {
        obstacleSpeed = speed;
    }
    
    private void Start()
    {
        currentSprite.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        cuttingPoint = spawners.cuttingPoint;
        obstacleSpeed = spawners.obstaclesSpeed;
        
        transform.localScale = new Vector3(transform.localScale.x * (Random.Range(0, 2) * 2 - 1), transform.localScale.y, transform.localScale.z);

    }

    void Update()
    {
        transform.position += Time.deltaTime * obstacleSpeed * Vector3.down;
        if (transform.position.y < cuttingPoint)
        {
            Destroy(gameObject);
        }
    }
}
