using UnityEngine;

public class InfinitePlaneScript : MonoBehaviour
{
    [SerializeField] private GameObject otherPlane;
    [SerializeField] private float planeLength = 10f;

    void Start()
    {

    }

    void Update()
    {
        float playerZ = Player.Instance.transform.position.z;
        float myZ = transform.position.z;
        float otherZ = otherPlane.transform.position.z;

        if (playerZ > myZ && otherZ < myZ)
        {
            otherPlane.transform.position = transform.position + Vector3.forward * planeLength;
        }
    }
}
