using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float turnSpeed = 10f;
    public float rotationSpeed = 360f;
    public float moveSpeed = 10f;

    public Transform[] wheels;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        RotateTank();
        RotateWheels();
    }

    void FixedUpdate()
    {
        MoveTank();
    }

    void MoveTank()
    {
        Vector3 move = transform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    void RotateTank()
    {
        float turn = horizontalInput * turnSpeed * moveSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void RotateWheels()
    {
        foreach (Transform wheel in wheels)
        {
            float wheelRotation = verticalInput * rotationSpeed * Time.deltaTime;

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
    }
}
