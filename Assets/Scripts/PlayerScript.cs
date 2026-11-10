using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float laneDistance = 7.5f;   // distance between lanes
    [SerializeField] private float laneSwitchSpeed = 30f; // how fast it slides to the new lane

    int currentLane = 0;          // -1 = left, 0 = middle, 1 = right
    float lastSidewaysInput = 0;

    void Update()
    {
        float sidewaysInput = Input.GetAxisRaw("Horizontal");

        if (sidewaysInput != 0 && lastSidewaysInput == 0)
        {
            currentLane = Mathf.Clamp(currentLane + (int)sidewaysInput, -1, 1);
        }
        lastSidewaysInput = sidewaysInput;

        Vector3 pos = transform.position;
        float targetX = currentLane * laneDistance;
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneSwitchSpeed * Time.deltaTime);
        pos.z += moveSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
