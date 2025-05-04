using UnityEngine;

public class TankCannon : TankWeapon
{
    public ProjectilePool bulletPool;
    public float launchForce = 20f;

    public override bool Fire()
    {
        if (!CanFire()) return false;

        Rigidbody bullet = bulletPool.GetProjectile();
        if (bullet == null) return false;

        bullet.transform.position = fireTransform.position;
        bullet.transform.rotation = fireTransform.rotation;
        bullet.gameObject.SetActive(true);
        bullet.linearVelocity = launchForce * fireTransform.forward;

        cooldownTimer = 0f;
        return true;
    }
}