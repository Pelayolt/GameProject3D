using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Transform fireTransform;
    public float launchForce = 15f;
    public float cooldownTime = 1f; // Tiempo entre disparos (en segundos)

    private float cooldownTimer;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && cooldownTimer >= cooldownTime)
        {
            Fire();
            cooldownTimer = 0f;  // Reiniciar el contador de cooldown
        }
    }

    void Fire()
    {
        Rigidbody bullet = BulletPool.Instance.GetBullet();

        if (bullet == null) return;

        bullet.transform.position = fireTransform.position;
        bullet.transform.rotation = fireTransform.rotation;
        bullet.gameObject.SetActive(true);

        bullet.linearVelocity = launchForce * fireTransform.forward;
    }
}