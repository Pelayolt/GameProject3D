using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float turnSpeed = 10;

    public float rotationSpeed = 360f;

    public float frontWheelsAngle = 30f;
    public float currentFrontWheelsAngle = 0f;

    public Transform[] wheels;
    
    public Transform frontRightWheel;
    public Transform frontLeftWheel;


    void Update(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput);
        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime);
        
        foreach (Transform wheel in wheels){
            wheel.Rotate(Vector3.right, verticalInput * rotationSpeed * Time.deltaTime);
        }

        currentFrontWheelsAngle = horizontalInput * frontWheelsAngle;

        frontLeftWheel.localRotation = Quaternion.Euler(0f, currentFrontWheelsAngle, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, currentFrontWheelsAngle, 0f);
        
    }
    
}
