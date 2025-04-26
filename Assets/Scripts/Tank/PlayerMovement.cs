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
    private Camera playerCamera; // Referencia a la cámara de jugador
    public Transform cameraTransform; // El transform de la cámara en primera persona

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        playerCamera = Camera.main;  // Asume que la cámara principal es la cámara de jugador
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        RotateTank();
        RotateWheels();

        // Mueve la cámara con el tanque
        MoveCameraWithTank();
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

    void MoveCameraWithTank()
    {
        if (cameraTransform != null)
        {
            // Desplazamos la cámara un poco más arriba (por ejemplo, 2 unidades en el eje Y)
            Vector3 offset = new Vector3(0f, 2f, 0f);  // Ajusta 2f a la altura que desees

            // La cámara sigue al tanque en la misma posición, pero con un desplazamiento en Y
            cameraTransform.position = transform.position + offset; // La cámara se mueve un poco más arriba
            cameraTransform.rotation = transform.rotation; // La cámara tiene la misma rotación que el tanque
        }
    }

}
