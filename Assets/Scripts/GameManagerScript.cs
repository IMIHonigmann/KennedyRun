using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private ObstacleScript obstacle; // drag the obstacle PREFAB here
    [SerializeField] private float obstacleRate = 10f;
    [SerializeField] private float obstacleSpawnDist = 40f;
    [SerializeField] private float laneDistance = 7.5f;
    [SerializeField] private GameOverMenuController gameOverMenu;
    [field: SerializeField] public int Score { get; private set; }


    private float nextSpawnZ;

    void Start()
    {
        nextSpawnZ = Player.Instance.transform.position.z + obstacleRate;
    }

    void Update()
    {
        Score = Mathf.FloorToInt(Player.Instance.transform.position.z);
        HandleObstacleSpawning();
    }

    private void HandleObstacleSpawning()
    {
        while (Player.Instance.transform.position.z >= nextSpawnZ)
        {
            int lane = Random.Range(-1, 2);
            Vector3 pos = new Vector3(lane * laneDistance, 1f, nextSpawnZ + obstacleSpawnDist);
            ObstacleScript o = Instantiate(obstacle, pos, Quaternion.identity);
            o.Init(this);
            nextSpawnZ += obstacleRate;
        }
    }

    public void StartGameOver()
    {
        gameOverMenu.ShowGameOver(Score);
    }
}
