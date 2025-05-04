using UnityEngine;

public class TankAimController : MonoBehaviour
{
    [Header("References")]
    public Transform turret;
    public Transform gun;
    public Camera thirdPersonCamera;

    [Header("First Person Settings")]
    public float mouseSensitivity = 20f;
    public float turretRotationSpeed = 5f;
    public float minGunElevation = -5f;
    public float maxGunElevation = 20f;

    [Header("Third Person Settings")]
    public float gunElevationSpeed = 5f;

    private float xRotation = 0f;

    public void HandleFirstPersonAim()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotar torreta
        turret.Rotate(Vector3.up * mouseX * turretRotationSpeed * Time.deltaTime);

        // Elevar/bajar el cañón
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minGunElevation, maxGunElevation);
        gun.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void HandleThirdPersonAim()
    {
        Ray ray = thirdPersonCamera.ScreenPointToRay(Input.mousePosition);
        int layerMask = ~(
            (1 << LayerMask.NameToLayer("IgnoreMainCamera")) |
            (1 << LayerMask.NameToLayer("TankPlayer"))
        );

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, layerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 targetPoint = hitInfo.point;

            // Rotar torreta en horizontal
            Vector3 directionToTarget = targetPoint - turret.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);

            // Ajustar elevación del cañón
            Vector3 localTarget = turret.InverseTransformPoint(targetPoint);
            float angle = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, minGunElevation, maxGunElevation);
            gun.localRotation = Quaternion.Lerp(gun.localRotation, Quaternion.Euler(angle, 0f, 0f), gunElevationSpeed * Time.deltaTime);
        }
    }
}