using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    void Update()
    {
        bool isAiming = Input.GetMouseButton(1);

        // Activamos una y desactivamos la otra, asegur�ndonos que nunca est�n las dos desactivadas
        firstPersonCamera.gameObject.SetActive(isAiming);
        thirdPersonCamera.gameObject.SetActive(!isAiming);
    }
}
