using UnityEngine;

public class RocketLife : MonoBehaviour
{
    public float lifetime = 5f;
    public GameObject explosionPrefab;
    public AudioClip impactClip;

    public float explosionRadius = 5f;
    public float explosionDamage = 50f;

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool hasCollided = false;
    private bool canCollide = false;

    public LayerMask CollisionLayer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnEnable()
    {
        audioSource.Stop();

        hasCollided = false;
        canCollide = false;
        CancelInvoke();

        SetVisualsActive(true);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Invoke(nameof(EnableCollision), 0.05f);     // Espera mínima tras activación
        Invoke(nameof(DisableRocket), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canCollide || hasCollided) return;
        hasCollided = true;

        Debug.Log($"🚀 Colisión con: {collision.collider.name}");

        // 🔥 1. Hacer daño en área
        Explode();

        // 🔥 2. Crear explosión visual
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        // 🔥 3. Detener física
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 🔥 4. Desactivar visuales
        SetVisualsActive(false);

        // 🔥 5. Sonido de impacto
        if (audioSource != null && impactClip != null)
        {
            audioSource.PlayOneShot(impactClip, 0.2f);
        }

        Invoke(nameof(DisableRocket), 3f);
    }

    private void EnableCollision()
    {
        canCollide = true;
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, CollisionLayer);

        foreach (var col in hits)
        {
            IDamageable dmg = col.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(explosionDamage);
                Debug.Log($"🔥 Daño en área a: {col.name}");
            }
        }
    }

    private void DisableRocket()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        gameObject.SetActive(false);
    }

    private void SetVisualsActive(bool isActive)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = isActive;

        foreach (var ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (isActive) ps.Play();
            else ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
