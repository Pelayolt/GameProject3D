using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowFirstPerson : MonoBehaviour
{
    [Header("References")]
    public Transform tankTurret;
    public Transform tankGun;
    public GameObject uiCrosshair;

    [Header("Settings")]
    public float mouseSensitivity = 2f;

    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        offset = Quaternion.Inverse(tankTurret.rotation) * (transform.position - tankTurret.position);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (uiCrosshair != null)
            uiCrosshair.SetActive(true);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (uiCrosshair != null)
            uiCrosshair.SetActive(false);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        tankTurret.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        float pitch = tankGun.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        transform.position = tankTurret.position + tankTurret.rotation * offset;

        Quaternion verticalRot = Quaternion.Euler(pitch, 0f, 0f);
        Quaternion horizontalRot = Quaternion.Euler(0f, tankTurret.eulerAngles.y, 0f);
        transform.rotation = horizontalRot * verticalRot;
    }
}
