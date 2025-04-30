using UnityEngine;

[System.Serializable]
public class SuspensionPoint
{
    public Transform suspensionPoint;
    public float springForce = 10000f;
    public float damper = 150f;
    public float restLength = 0.5f;

    [HideInInspector] public float lastLength;
}

[RequireComponent(typeof(Rigidbody))]
public class TankSuspension : MonoBehaviour
{
    public SuspensionPoint[] suspensionPoints;
    public LayerMask groundLayer;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach (var susp in suspensionPoints)
        {
            RaycastHit hit;
            Vector3 origin = susp.suspensionPoint.position;
            Vector3 direction = -susp.suspensionPoint.up;

            if (Physics.Raycast(origin, direction, out hit, susp.restLength, groundLayer))
            {
                float currentLength = hit.distance;
                float springVelocity = (susp.lastLength - currentLength) / Time.fixedDeltaTime;
                float forceMagnitude = (susp.restLength - currentLength) * susp.springForce - springVelocity * susp.damper;

                rb.AddForceAtPosition(susp.suspensionPoint.up * forceMagnitude, origin);

                susp.lastLength = currentLength;
            }
            else
            {
                // Si no está tocando el suelo, guardamos la longitud máxima
                susp.lastLength = susp.restLength;
            }
        }
    }
}
