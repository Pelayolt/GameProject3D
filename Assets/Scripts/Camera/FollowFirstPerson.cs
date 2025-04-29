using UnityEngine;

public class FollowFirstPerson : MonoBehaviour
{
    public Transform tankTurret;
    public float followSpeed = 5f;
    public Camera firstPersonCamera;
    public float mouseSensitivity = 2f;

    private float xRotation = 0f;
    private Vector3 offset;

    void Start()
    {
        // Guardar el offset inicial
        offset = Quaternion.Inverse(tankTurret.rotation) * (transform.position - tankTurret.position);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!firstPersonCamera.gameObject.activeSelf) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotar horizontalmente el tanque
        tankTurret.Rotate(Vector3.up * mouseX);

        // Acumular rotación vertical (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
    }

    void LateUpdate()
    {
        if (!firstPersonCamera.gameObject.activeSelf) return;

        // Posición relativa al tanque
        Vector3 desiredPosition = tankTurret.position + tankTurret.rotation * offset;
        transform.position = desiredPosition;

        // Rotación: vertical controlada por ratón, horizontal por la torreta
        Quaternion verticalRot = Quaternion.Euler(xRotation, 0f, 0f);
        Quaternion horizontalRot = Quaternion.Euler(0f, tankTurret.eulerAngles.y, 0f);
        transform.rotation = horizontalRot * verticalRot;
    }
}
