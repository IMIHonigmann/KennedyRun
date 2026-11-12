using System;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float despawnOffset;
    [SerializeField] private bool allowDespawn = true;
    [SerializeField] private GameManagerScript gameManager;

    // Called by ObstacleSpawner right after spawning, since a prefab can't reference scene objects.
    public void Init(GameObject player, GameManagerScript gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z - despawnOffset > transform.position.z && allowDespawn)
        {
            Destroy(gameObject, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject == player)
        {
            Debug.Log("Collided with " + other.gameObject);
            gameManager.StartGameOver();
        }
    }
}
