using UnityEngine;

public class TankCameraControl : MonoBehaviour
{
    public Transform turret;   // Parte que rota horizontalmente (la torreta)
    public Transform gun;      // Parte que sube y baja (el tubo del cañón)
    public Camera firstPersonCamera;  // Cámara en primera persona
    public Camera thirdPersonCamera;  // Cámara en tercera persona
    public float turretRotationSpeed = 5f;
    public float gunElevationSpeed = 5f;
    public float minGunElevation = -5f; // grados
    public float maxGunElevation = 20f; // grados
    public float cameraSensitivity = 2f; // Sensibilidad de la cámara para movimiento de ratón

    private bool isInFirstPerson = false;
    private Vector3 initialCameraOffset; // Almacenamos la posición inicial de la cámara

    void Start()
    {
        // Configurar la cámara en primera persona y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Establecemos la posición inicial de la cámara en primera persona
        if (firstPersonCamera != null)
        {
            initialCameraOffset = new Vector3(0.3f, 0.5f, -0.5f); // Ajustar esta posición para que la cámara esté detrás y arriba del cañón
        }
    }

    void Update()
    {
        // Detectar si el clic derecho está presionado
        if (Input.GetMouseButton(1)) // 1 es el clic derecho
        {
            ActivateFirstPerson();
        }
        else
        {
            ActivateThirdPerson();
        }

        // Si estamos en primera persona, mover el cañón y la cámara
        if (isInFirstPerson)
        {
            AimAtMouseFirstPerson(); // Mueve el cañón hacia el movimiento del ratón
            MoveCameraWithMouse();  // Mueve la cámara con el ratón
        }
        else
        {
            AimAtMouseThirdPerson();  // Apuntar el cañón en tercera persona
        }
    }

    void ActivateFirstPerson()
    {
        isInFirstPerson = true;
        thirdPersonCamera.gameObject.SetActive(false); // Desactivar cámara en tercera persona
        firstPersonCamera.gameObject.SetActive(true);  // Activar cámara en primera persona
    }

    void ActivateThirdPerson()
    {
        isInFirstPerson = false;
        thirdPersonCamera.gameObject.SetActive(true);  // Activar cámara en tercera persona
        firstPersonCamera.gameObject.SetActive(false); // Desactivar cámara en primera persona
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // En primera persona, el cañón y la cámara siguen el movimiento del ratón
    void AimAtMouseFirstPerson()
    {
        // Obtén el movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 1. Rotar la torreta en horizontal (solo en Y) en función del movimiento del ratón
        turret.Rotate(Vector3.up * mouseX * turretRotationSpeed);

        // 2. Elevar o bajar el cañón (solo en X) en función del movimiento del ratón
        float newAngle = gun.localRotation.eulerAngles.x - mouseY * cameraSensitivity;
        newAngle = Mathf.Clamp(newAngle, minGunElevation, maxGunElevation);
        gun.localRotation = Quaternion.Euler(newAngle, 0f, 0f);
    }

    // En tercera persona, el cañón sigue el raycast del ratón
    void AimAtMouseThirdPerson()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f))
        {
            Vector3 targetPoint = hitInfo.point;

            // 1. Rotar la torreta en horizontal (solo en Y)
            Vector3 directionToTarget = targetPoint - turret.position;
            directionToTarget.y = 0;  // Solo rotar en plano horizontal
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);

            // 2. Elevar o bajar el cañón (sólo en X)
            Vector3 localTarget = turret.InverseTransformPoint(targetPoint);

            // Calculamos el ángulo de elevación
            float angle = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;

            // Aseguramos que el ángulo esté dentro del rango permitido
            angle = Mathf.Clamp(angle, minGunElevation, maxGunElevation);

            // Aplicamos la rotación del cañón
            gun.localRotation = Quaternion.Lerp(gun.localRotation, Quaternion.Euler(angle, 0f, 0f), gunElevationSpeed * Time.deltaTime);
        }
    }

    // En primera persona, movemos la cámara con el ratón
    void MoveCameraWithMouse()
    {
        // Movimiento de la cámara con el ratón
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotar la cámara y el cañón en base al movimiento del ratón
        firstPersonCamera.transform.Rotate(Vector3.up * mouseX * cameraSensitivity);
        firstPersonCamera.transform.Rotate(Vector3.left * mouseY * cameraSensitivity);

        // Aseguramos que la cámara esté alineada con el cañón
        firstPersonCamera.transform.rotation = gun.rotation;  // Mantener la rotación de la cámara alineada con la del cañón
    }
}
