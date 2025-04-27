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
    private Camera playerCamera; // Referencia a la c�mara de jugador
    public Transform cameraTransform; // El transform de la c�mara en primera persona

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        playerCamera = Camera.main;  // Asume que la c�mara principal es la c�mara de jugador
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        RotateTank();
        RotateWheels();

        // Mueve la c�mara con el tanque
        MoveCameraWithTank();
    }

    void FixedUpdate()
    {
        MoveTank();
    }

    void MoveTank()
    {
        Vector3 move = transform.forward * verticalInput * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z); // Mantener la velocidad vertical para la gravedad
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
            Vector3 offset = new Vector3(0f, 2f, 0f);

            // La c�mara sigue al tanque en la misma posici�n, pero con un desplazamiento en Y
            cameraTransform.position = transform.position + offset; // La c�mara se mueve un poco m�s arriba
            cameraTransform.rotation = transform.rotation; // La c�mara tiene la misma rotaci�n que el tanque
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            Rigidbody otherRb = collision.rigidbody;

            if (otherRb == null)
            {
                Debug.LogWarning("El objeto destructible no tiene Rigidbody");
                return;
            }

            // Activar la física
            otherRb.isKinematic = false;
            otherRb.constraints = RigidbodyConstraints.None;

            Vector3 forceDirection = collision.transform.position - transform.position;
            forceDirection.y = 0.2f; // Muy poquito hacia arriba
            otherRb.AddForce(forceDirection.normalized * 5f, ForceMode.Impulse); // Mucha menos fuerza
            

            // Cambiar la capa para que ya no bloquee
            collision.gameObject.layer = LayerMask.NameToLayer("NoCollide");
        }
    }


}
