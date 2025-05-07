using UnityEngine;

public class TankCannon : TankWeapon
{
    public ProjectilePool bulletPool;
    public AudioSource audioSource;
    public AudioClip fireClip;
    public float launchForce = 20f;

    void OnEnable()
    {
        if (audioSource != null)
        {
            audioSource.clip = fireClip;
            audioSource.Stop();
        }
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        if (audioSource != null && fireClip != null)
        {
            audioSource.PlayOneShot(fireClip, 0.20f);
        }

        Rigidbody bullet = bulletPool.GetProjectile();
        if (bullet == null) return;

        bullet.transform.position = fireTransform.position;
        bullet.transform.rotation = fireTransform.rotation;
        bullet.gameObject.SetActive(true);
        bullet.linearVelocity = launchForce * fireTransform.forward;

        cooldownTimer = 0f;
    }
}