using System.Collections;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private float despawnOffset;
    [SerializeField] private float sonClipDelay = 0.5f; // seconds after the explosion
    [SerializeField] private bool allowDespawn = true;
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private GameObject explosion;

    [SerializeField] private AudioClip theyDontKnowMeSonClip;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float perfectDodgeDistance;
    [SerializeField] private float laneHalfWidth = 3.75f;

    [Header("Perfect dodge launch")]
    [SerializeField] private float launchForce = 25f;
    [SerializeField] private float launchSpin = 15f;
    [SerializeField] private float launchLifetime = 3f;

    private Collider obstacleCollider;
    private bool launched;

    private bool wasInPlayersLaneWhenClose;
    private bool dodgeResolved;
    private AudioSource playerAudio;

    public void Init(GameManagerScript gameManager)
    {
        this.gameManager = gameManager;
        playerAudio = Player.Instance.GetComponent<AudioSource>();
        obstacleCollider = GetComponent<Collider>();
    }

    private static IEnumerator PlayDelayed(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!GameOverMenuController.IsGameOver)
            source.PlayOneShot(clip);
    }

    private void LaunchAway()
    {
        launched = true;

        obstacleCollider.isTrigger = false;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        float side = Mathf.Sign(transform.position.x - Player.Instance.transform.position.x);
        Vector3 dir = new Vector3(side * 0.15f, 0.25f, 1f).normalized;
        rb.linearVelocity = dir * (Player.Instance.MoveSpeed + launchForce);
        rb.AddTorque(UnityEngine.Random.onUnitSphere * launchSpin, ForceMode.Impulse);
        GetComponent<Renderer>().material.SetColor("_base_color", Color.green);

        Destroy(gameObject, launchLifetime);
    }

    void Update()
    {
        float distanceAhead = transform.position.z - Player.Instance.transform.position.z; // > 0 while obstacle is ahead

        if (distanceAhead > 0 && distanceAhead < perfectDodgeDistance &&
            Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) < laneHalfWidth)
        {
            wasInPlayersLaneWhenClose = true;
        }

        if (!dodgeResolved && distanceAhead <= 0)
        {
            dodgeResolved = true;
            if (wasInPlayersLaneWhenClose && !GameOverMenuController.IsGameOver)
            {
                bool rareLoud = Random.Range(0, 1000) == 0;
                playerAudio.PlayOneShot(explosionSound, rareLoud ? 2 : 1);
                Player.Instance.StartCoroutine(PlayDelayed(playerAudio, theyDontKnowMeSonClip, sonClipDelay));
                LaunchAway();
                GameObject o = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(o, 5f);
            }
        }

        // Launched obstacles clean themselves up after launchLifetime instead
        if (!launched && Player.Instance.transform.position.z - despawnOffset > transform.position.z && allowDespawn)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject == Player.Instance.gameObject)
        {
            Debug.Log("Collided with " + other.gameObject);
            gameManager.StartGameOver();
        }
    }
}
