using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Transform player;
    [SerializeField] private float obstacleRate = 10f;
    [SerializeField] private float obstacleSpawnDist = 40f;
    [SerializeField] private float laneDistance = 7.5f;

    private float nextSpawnZ;

    void Start()
    {
        nextSpawnZ = player.position.z + obstacleRate;
    }

    void Update()
    {
        while (player.position.z >= nextSpawnZ)
        {
            int lane = Random.Range(-1, 2);
            Vector3 pos = new Vector3(lane * laneDistance, 0f, nextSpawnZ + obstacleSpawnDist);
            GameObject o = Instantiate(obstacle, pos, Quaternion.identity);
            nextSpawnZ += obstacleRate;
        }
    }
}
