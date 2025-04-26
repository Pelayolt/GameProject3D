using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // El tanque o el objeto que la cámara debe seguir
    public float smoothSpeed = 0.125f;  // Qué tan suave debe ser el movimiento
    public Vector3 offset;  // Distancia entre la cámara y el tanque

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;  // La posición deseada con el offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // Suaviza el movimiento
        transform.position = smoothedPosition;  // Actualiza la posición de la cámara
    }
}
