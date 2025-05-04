using UnityEngine;

public class RocketLife : MonoBehaviour
{
    public float lifetime = 5f;            // Cohetes suelen vivir más tiempo
    public GameObject explosionPrefab;     // Prefab de la explosión del cohete

    public AudioClip impactClip;           // Sonido de impacto
    private AudioSource audioSource;

    public float explosionRadius = 5f; // Parametros de explosion
    public float explosionDamage = 50f;

    private Rigidbody rb;
    private bool hasCollided = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //Para detectar bien las colisiones
    }

    private void OnEnable()
    {
        hasCollided = false; // Reseteamos al reutilizar del pool
        CancelInvoke();
        Invoke(nameof(DisableRocket), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return; // Evitar múltiples impactos
        hasCollided = true;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Dejar la explosión un poco más tiempo
        }

        // Explode();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Congelar
        }

        if (audioSource != null && impactClip != null)
        {
            audioSource.PlayOneShot(impactClip);
        }

        Invoke(nameof(DisableRocket), 0.7f); // Un poco más de tiempo que la bala
    }

    private void DisableRocket()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivar física para el pool
        }
        gameObject.SetActive(false);
    }

    // private void Explode()
    // {
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

    //     foreach (Collider nearbyObject in colliders)
    //     {
    //         // Comprobar si puede recibir daño
    //         Health targetHealth = nearbyObject.GetComponent<Health>();

    //         if (targetHealth != null)
    //         {
    //             targetHealth.TakeDamage(explosionDamage);
    //         }
    //     }
    // }
}
