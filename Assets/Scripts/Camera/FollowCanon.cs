using UnityEngine;

public class FirstPersonCannonCamera : MonoBehaviour
{
    public Transform tankTurret;    // Referencia al cañón (torreta) del tanque
    public float followSpeed = 5f;  // Velocidad de seguimiento de la cámara
    public Vector3 offset;  // Offset de la cámara respecto al cañón (posición detrás del cañón)

    void LateUpdate()
    {
        // 1. Colocamos la cámara en la posición deseada detrás del cañón, con el offset
        Vector3 desiredPosition = tankTurret.position + tankTurret.TransformDirection(offset);

        // 2. Suavizamos el movimiento de la cámara hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 3. Hacemos que la cámara mire en la misma dirección en la que apunta el cañón
        transform.rotation = tankTurret.rotation; // Usamos la rotación de la torreta para la cámara
    }
}
