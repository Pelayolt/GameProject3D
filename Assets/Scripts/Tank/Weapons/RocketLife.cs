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

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        SetVisualsActive(false);

        if (audioSource != null && impactClip != null)
        {
            audioSource.PlayOneShot(impactClip, 0.20f);
        }

        Invoke(nameof(DisableRocket), 3f);
    }

    private void OnEnable()
    {
        audioSource.Stop();

        hasCollided = false;
        CancelInvoke();
        Invoke(nameof(DisableRocket), lifetime);

        SetVisualsActive(true);

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void SetVisualsActive(bool isActive)
    {
        // Desactiva todos los renderers y particle systems
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = isActive;

        foreach (var ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (isActive) ps.Play();
            else ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void DisableRocket()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivar física para el pool
        }
        gameObject.SetActive(false);
    }
}
