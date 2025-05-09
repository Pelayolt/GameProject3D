using UnityEngine;
using System.Collections;

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

        RotateWheels();
    }

    void FixedUpdate()
    {
        MoveTank();
    }

    void MoveTank()
    {
        if(Time.deltaTime == 0)
            return;
        float turn = horizontalInput * turnSpeed * moveSpeed * Time.deltaTime;

        Vector3 move = transform.forward * verticalInput * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z); // Mantener la velocidad vertical para la gravedad

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

            // Opcional: aplicar fuerza leve
            Vector3 forceDirection = collision.transform.position - transform.position;
            forceDirection.y = 0.1f;
            otherRb.AddForce(forceDirection.normalized * 3f, ForceMode.Impulse);

            // Iniciar corrutina para cambiar de capa después de 2 segundos
            StartCoroutine(ChangeLayerAfterDelay(collision.gameObject, 2f));
        }
    }

    private IEnumerator ChangeLayerAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.layer = LayerMask.NameToLayer("NoCollide");
    }


}
