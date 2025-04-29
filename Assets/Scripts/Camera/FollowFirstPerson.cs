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
        // Calcular el offset automáticamente basado en la posición inicial en el mundo
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

        tankTurret.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, tankTurret.eulerAngles.y, 0f);
    }

    void LateUpdate()
    {
        if (!firstPersonCamera.gameObject.activeSelf) return;

        Vector3 desiredPosition = tankTurret.position + tankTurret.rotation * offset;
        transform.position = desiredPosition;

        // La rotación vertical se mantiene igual
        transform.localRotation = Quaternion.Euler(xRotation, tankTurret.eulerAngles.y, 0f);
    }

}
