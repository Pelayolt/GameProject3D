using UnityEngine;

public class FollowThirdPerson : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Camera thirdPersonCamera;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (!thirdPersonCamera.gameObject.activeSelf) return; // No seguir si está desactivada

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}