using UnityEngine;

public class FirstPersonCannonCamera : MonoBehaviour
{
    public Transform tankTurret;    // Referencia al ca��n (torreta) del tanque
    public float followSpeed = 5f;  // Velocidad de seguimiento de la c�mara
    public Vector3 offset;  // Offset de la c�mara respecto al ca��n (posici�n detr�s del ca��n)

    void LateUpdate()
    {
        // 1. Colocamos la c�mara en la posici�n deseada detr�s del ca��n, con el offset
        Vector3 desiredPosition = tankTurret.position + tankTurret.TransformDirection(offset);

        // 2. Suavizamos el movimiento de la c�mara hacia la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 3. Hacemos que la c�mara mire en la misma direcci�n en la que apunta el ca��n
        transform.rotation = tankTurret.rotation; // Usamos la rotaci�n de la torreta para la c�mara
    }
}
