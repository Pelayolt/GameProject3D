using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    public float rotationSpeed = 45f;   // Grados por segundo
    public float floatAmplitude = 0.25f; // Qué tanto sube y baja
    public float floatFrequency = 1f;   // Velocidad de flotación

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotación continua en Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento vertical tipo seno
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
