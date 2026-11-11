using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject player;
    [SerializeField] private float obstacleRate;
    [SerializeField] private float obstacleSpawnDist;
    private bool hasSpawned;
    void Start()
    {

    }

    void Update()
    {
        float obstacleTimer = Mathf.Abs(player.transform.position.z) % obstacleRate;

        if (obstacleTimer > obstacleRate - 1 && hasSpawned == false)
        {
            Instantiate(obstacle, new Vector3(Mathf.Round(Random.Range(-1.49f, 1.49f)) * 7.5f, 0, player.transform.position.z + obstacleSpawnDist), Quaternion.identity);
            hasSpawned = true;
        }
        if (obstacleTimer < 1)
        {
            hasSpawned = false;
        }
        Debug.Log(obstacleTimer);
    }
}
