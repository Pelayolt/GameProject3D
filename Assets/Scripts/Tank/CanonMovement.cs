using UnityEngine;

public class TankTurretAiming : MonoBehaviour
{
    public Transform turret;   // Parte que rota horizontalmente (la torreta)
    public Transform gun;      // Parte que sube y baja (el tubo del cañón)

    public float turretRotationSpeed = 5f;
    public float gunElevationSpeed = 5f;

    public float minGunElevation = -5f; // grados
    public float maxGunElevation = 20f;

    public LayerMask aimLayerMask; // Para limitar a qué superficie puede apuntar (suelo)

    private float currentGunElevation = 0f;

    void Update()
    {
        AimAtMouse();
    }

    void AimAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, aimLayerMask))
        {
            Vector3 targetPoint = hitInfo.point;

            // 1. Rotar la torreta en horizontal (solo en Y)
            Vector3 directionToTarget = targetPoint - turret.position;
            directionToTarget.y = 0; // Solo rotar en plano horizontal

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);

            // 2. Elevar o bajar el cañón (sólo en X)
            Vector3 localTarget = turret.InverseTransformPoint(targetPoint);
            float angle = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;

            angle = Mathf.Clamp(angle, minGunElevation, maxGunElevation);

            gun.localRotation = Quaternion.Lerp(gun.localRotation, Quaternion.Euler(angle, 0f, 0f), gunElevationSpeed * Time.deltaTime);
        }
    }
}
