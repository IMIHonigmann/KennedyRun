using System;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float despawnOffset;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z - despawnOffset > transform.position.z)
        {
            Destroy(gameObject, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.gameObject == player)
        {
            Debug.Log("Collided with " + other.gameObject);
        }
    }
}
