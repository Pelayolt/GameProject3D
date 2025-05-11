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
        CancelInvoke();

        SetVisualsActive(true);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Invoke(nameof(DisableRocket), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        Debug.Log($"🚀 Colisión con: {collision.collider.name}");
        Explode();

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
            audioSource.PlayOneShot(impactClip, 0.2f);
        }

        Invoke(nameof(DisableRocket), 3f);
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, CollisionLayer);
        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}");
        }
        foreach (var col in hits)
        {
            IDamageable dmg = col.GetComponent<IDamageable>();
            if (dmg == null)
                dmg = col.GetComponentInParent<IDamageable>();
            if (dmg == null)
                dmg = col.GetComponentInChildren<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(explosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void DisableRocket()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
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

        Collider col = GetComponent<Collider>();
        if (col != null)
                col.enabled = isActive;

    }
}
