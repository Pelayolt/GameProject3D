using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    [Header("Aiming")]
    public TankAimController aimHandler;

    private bool isFirstPerson;

    void Start()
    {
        SetFirstPerson(false); // Empieza en tercera persona
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetFirstPerson(!isFirstPerson);
        }

        if (isFirstPerson)
            aimHandler.HandleFirstPersonAim();
        else
            aimHandler.HandleThirdPersonAim();
    }

    void SetFirstPerson(bool value)
    {
        isFirstPerson = value;

        if (firstPersonCamera != null) firstPersonCamera.gameObject.SetActive(value);
        if (thirdPersonCamera != null) thirdPersonCamera.gameObject.SetActive(!value);
    }
}
