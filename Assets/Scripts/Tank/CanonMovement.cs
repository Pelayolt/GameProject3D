using UnityEngine;

public class TankCameraControl : MonoBehaviour
{
    public Transform turret;   // Parte que rota horizontalmente (la torreta)
    public Transform gun;      // Parte que sube y baja (el tubo del ca��n)
    public Camera firstPersonCamera;  // C�mara en primera persona
    public Camera thirdPersonCamera;  // C�mara en tercera persona
    public float turretRotationSpeed = 5f;
    public float gunElevationSpeed = 5f;
    public float minGunElevation = -5f; // grados
    public float maxGunElevation = 20f; // grados
    public float cameraSensitivity = 2f; // Sensibilidad de la c�mara para movimiento de rat�n

    private bool isInFirstPerson = false;
    private Vector3 initialCameraOffset; // Almacenamos la posici�n inicial de la c�mara

    void Start()
    {
        // Configurar la c�mara en primera persona y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Establecemos la posici�n inicial de la c�mara en primera persona
        if (firstPersonCamera != null)
        {
            initialCameraOffset = new Vector3(0.3f, 0.5f, -0.5f); // Ajustar esta posici�n para que la c�mara est� detr�s y arriba del ca��n
        }
    }

    void Update()
    {
        // Detectar si el clic derecho est� presionado
        if (Input.GetMouseButton(1)) // 1 es el clic derecho
        {
            ActivateFirstPerson();
        }
        else
        {
            ActivateThirdPerson();
        }

        // Si estamos en primera persona, mover el ca��n y la c�mara
        if (isInFirstPerson)
        {
            AimAtMouseFirstPerson(); // Mueve el ca��n hacia el movimiento del rat�n
            MoveCameraWithMouse();  // Mueve la c�mara con el rat�n
        }
        else
        {
            AimAtMouseThirdPerson();  // Apuntar el ca��n en tercera persona
        }
    }

    void ActivateFirstPerson()
    {
        isInFirstPerson = true;
        thirdPersonCamera.gameObject.SetActive(false); // Desactivar c�mara en tercera persona
        firstPersonCamera.gameObject.SetActive(true);  // Activar c�mara en primera persona
    }

    void ActivateThirdPerson()
    {
        isInFirstPerson = false;
        thirdPersonCamera.gameObject.SetActive(true);  // Activar c�mara en tercera persona
        firstPersonCamera.gameObject.SetActive(false); // Desactivar c�mara en primera persona
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // En primera persona, el ca��n y la c�mara siguen el movimiento del rat�n
    void AimAtMouseFirstPerson()
    {
        // Obt�n el movimiento del rat�n
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 1. Rotar la torreta en horizontal (solo en Y) en funci�n del movimiento del rat�n
        turret.Rotate(Vector3.up * mouseX * turretRotationSpeed);

        // 2. Elevar o bajar el ca��n (solo en X) en funci�n del movimiento del rat�n
        float newAngle = gun.localRotation.eulerAngles.x - mouseY * cameraSensitivity;
        newAngle = Mathf.Clamp(newAngle, minGunElevation, maxGunElevation);
        gun.localRotation = Quaternion.Euler(newAngle, 0f, 0f);
    }

    // En tercera persona, el ca��n sigue el raycast del rat�n
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

            // 2. Elevar o bajar el ca��n (s�lo en X)
            Vector3 localTarget = turret.InverseTransformPoint(targetPoint);

            // Calculamos el �ngulo de elevaci�n
            float angle = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;

            // Aseguramos que el �ngulo est� dentro del rango permitido
            angle = Mathf.Clamp(angle, minGunElevation, maxGunElevation);

            // Aplicamos la rotaci�n del ca��n
            gun.localRotation = Quaternion.Lerp(gun.localRotation, Quaternion.Euler(angle, 0f, 0f), gunElevationSpeed * Time.deltaTime);
        }
    }

    // En primera persona, movemos la c�mara con el rat�n
    void MoveCameraWithMouse()
    {
        // Movimiento de la c�mara con el rat�n
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotar la c�mara y el ca��n en base al movimiento del rat�n
        firstPersonCamera.transform.Rotate(Vector3.up * mouseX * cameraSensitivity);
        firstPersonCamera.transform.Rotate(Vector3.left * mouseY * cameraSensitivity);

        // Aseguramos que la c�mara est� alineada con el ca��n
        firstPersonCamera.transform.rotation = gun.rotation;  // Mantener la rotaci�n de la c�mara alineada con la del ca��n
    }
}
