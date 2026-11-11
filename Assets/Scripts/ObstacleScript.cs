using System;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.gameObject == player)
        {
            Debug.Log("Collided with " + other.gameObject);
        }
    }
}
