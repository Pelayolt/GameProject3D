using UnityEngine;

public class BulletLife : MonoBehaviour
{
    public float lifetime = 2f;
    public GameObject explosionPrefab; // Prefab de la explosión

    public AudioClip impactClip;
    private AudioSource audioSource;

    private Rigidbody rb;
    private bool hasCollided = false;
    private bool canCollide = false;
    public float damage = 10f;

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

        Debug.Log("Colision con: " + collision.collider.name);
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
        if (dmg == null)
            dmg = collision.gameObject.GetComponentInParent<IDamageable>();
        if (dmg == null)
            dmg = collision.gameObject.GetComponentInChildren<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            Debug.Log("Daño por golpeo con: " + collision.collider.name);
        }

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

        Invoke(nameof(DisableBullet), 3f);
    }

    private void OnEnable()
    {
        CancelInvoke();

        SetVisualsActive(true);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Invoke(nameof(DisableBullet), lifetime);
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

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = isActive;
    }

    private void DisableBullet()
    {
        hasCollided = false;
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivar física cuando vuelva al pool
        }
        gameObject.SetActive(false);
    }
}