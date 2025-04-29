using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Transform fireTransform;
    public float launchForce = 80f;
    private float cooldownTimer = 0f;
    public float cooldownTime = 1f; // Tiempo entre disparos (en segundos)
    

    public AudioSource audioSource;      // 🔊 Fuente de sonido
    public AudioClip fireClip;            // 🎶 Sonido de disparo


    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    public void Fire()
    {
        if (cooldownTimer >= cooldownTime)
        {
            Rigidbody bullet = BulletPool.Instance.GetBullet();

            if (bullet == null) return;

            bullet.transform.position = fireTransform.position;
            bullet.transform.rotation = fireTransform.rotation;
            bullet.gameObject.SetActive(true);

            bullet.linearVelocity = launchForce * fireTransform.forward;

            if (audioSource != null && fireClip != null)
            {
                audioSource.PlayOneShot(fireClip);
            }
            cooldownTimer = 0f;// Reiniciar el contador de cooldown
        }
    }
}