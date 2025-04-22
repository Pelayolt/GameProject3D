using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject cannon;
    public float rotateSpeed = 5;

    public float cannonRotation = 0.0f;

    private const float minCannonRotation = -20.0f;
    private const float maxCannonRotation = 20.0f;

    Vector3 offset;
    void Start() {
        offset = player.transform.position - transform.position;
    }



    void LateUpdate()
    {

        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        
        player.transform.Rotate(0, horizontal, 0);
        
        cannonRotation -= vertical;
        cannonRotation = Mathf.Clamp(cannonRotation, minCannonRotation, maxCannonRotation);
        cannon.transform.Rotate(vertical, 0, 0);

        float desiredAngle = player.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(cannonRotation, desiredAngle, 0);
        transform.position = player.transform.position - (rotation * offset);
        transform.LookAt(player.transform);

     

    }
}
