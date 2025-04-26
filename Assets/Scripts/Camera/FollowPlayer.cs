using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // El tanque
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Posición relativa

    public float followSpeed = 5f;   // Qué tan rápido sigue

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }
}
