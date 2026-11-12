using System;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float despawnOffset;
    [SerializeField] private bool allowDespawn = true;
    [SerializeField] private GameManagerScript gameManager;

    [SerializeField] private AudioClip theyDontKnowMeSonClip;
    [SerializeField] private float perfectDodgeDistance;
    [SerializeField] private float laneHalfWidth = 3.75f;

    private bool wasInPlayersLaneWhenClose;
    private bool dodgeResolved;
    private AudioSource playerAudio;

    public void Init(GameObject player, GameManagerScript gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;
        playerAudio = player.GetComponent<AudioSource>();
    }

    void Update()
    {
        float distanceAhead = transform.position.z - player.transform.position.z; // > 0 while obstacle is ahead

        if (distanceAhead > 0 && distanceAhead < perfectDodgeDistance &&
            Mathf.Abs(player.transform.position.x - transform.position.x) < laneHalfWidth)
        {
            wasInPlayersLaneWhenClose = true;
        }

        if (!dodgeResolved && distanceAhead <= 0)
        {
            dodgeResolved = true;
            if (wasInPlayersLaneWhenClose && !GameOverMenuController.IsGameOver)
                playerAudio.PlayOneShot(theyDontKnowMeSonClip);
        }

        if (player.transform.position.z - despawnOffset > transform.position.z && allowDespawn)
        {
            Destroy(gameObject);
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
