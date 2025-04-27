using UnityEngine;

public class BulletLife : MonoBehaviour
{
    public float lifetime = 2f;
    public GameObject explosionPrefab; // Prefab de la explosión

    public AudioClip impactClip;
    private AudioSource audioSource;

    private Rigidbody rb;
    private bool hasCollided = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        hasCollided = false; // Reset al reutilizar del pool
        CancelInvoke();
        Invoke(nameof(DisableBullet), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return; // Evitar múltiples colisiones
        hasCollided = true;

        // 🔥 1. Crear explosión si hay
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }

        // 🔥 2. Quitar la física para que no rebote
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Lo congelamos
        }

        // 🔥 3. Reproducir sonido de impacto
        if (audioSource != null && impactClip != null)
        {
            audioSource.PlayOneShot(impactClip);
        }

        // 🔥 4. Desactivar la bala después de un corto tiempo
        Invoke(nameof(DisableBullet), 0.5f); // Un poco de tiempo para que suene
    }

    private void DisableBullet()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivar física cuando vuelva al pool
        }
        gameObject.SetActive(false);
    }
}