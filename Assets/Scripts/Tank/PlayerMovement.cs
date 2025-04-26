using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float turnSpeed = 10;

    public float rotationSpeed = 360f;

    public float frontWheelsAngle = 30f;
    public float currentFrontWheelsAngle = 0f;

    public float moveSpeed = 10f;

    public Transform[] wheels;

    public Transform frontRightWheel;
    public Transform frontLeftWheel;


    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Movimiento del tanque
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * verticalInput);
        transform.Rotate(Vector3.up * moveSpeed * horizontalInput * turnSpeed * Time.deltaTime);

        // Ruedas girando
        foreach (Transform wheel in wheels)
        {
            float wheelRotation = verticalInput * rotationSpeed * Time.deltaTime;

            // Si está girando, sumar o restar según el lado de la rueda
            if (verticalInput == 0 && horizontalInput != 0)
            {
                if (wheel.name.Contains("left"))
                {
                    wheelRotation = horizontalInput * rotationSpeed * Time.deltaTime;
                }
                else if (wheel.name.Contains("right"))
                {
                    wheelRotation = -horizontalInput * rotationSpeed * Time.deltaTime;
                }
            }

            wheel.Rotate(Vector3.right, wheelRotation);
        }

        // Direccionar las ruedas delanteras
        currentFrontWheelsAngle = horizontalInput * frontWheelsAngle;
        frontLeftWheel.localRotation = Quaternion.Euler(0f, currentFrontWheelsAngle, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, currentFrontWheelsAngle, 0f);
    }


}
