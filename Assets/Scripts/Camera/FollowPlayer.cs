using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // El tanque o el objeto que la c�mara debe seguir
    public float smoothSpeed = 0.125f;  // Qu� tan suave debe ser el movimiento
    public Vector3 offset;  // Distancia entre la c�mara y el tanque

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;  // La posici�n deseada con el offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // Suaviza el movimiento
        transform.position = smoothedPosition;  // Actualiza la posici�n de la c�mara
    }
}
