using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private ObstacleScript obstacle; // drag the obstacle PREFAB here
    [SerializeField] private Transform player;
    [SerializeField] private float obstacleRate = 10f;
    [SerializeField] private float obstacleSpawnDist = 40f;
    [SerializeField] private float laneDistance = 7.5f;
    [SerializeField] private int highScore;

    private float nextSpawnZ;

    void Start()
    {
        nextSpawnZ = player.position.z + obstacleRate;
    }

    void Update()
    {
        highScore = Mathf.FloorToInt(player.transform.position.z);
        HandleObstacleSpawning();
    }

    private void HandleObstacleSpawning()
    {
        while (player.position.z >= nextSpawnZ)
        {
            int lane = Random.Range(-1, 2);
            Vector3 pos = new Vector3(lane * laneDistance, 0f, nextSpawnZ + obstacleSpawnDist);
            ObstacleScript o = Instantiate(obstacle, pos, Quaternion.identity);
            o.Init(player.gameObject);
            nextSpawnZ += obstacleRate;
        }
    }
}
