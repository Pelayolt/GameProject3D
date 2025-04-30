using UnityEngine;

public class RocketLauncher : TankWeapon
{
    public ProjectilePool rocketPool;
    public float launchForce = 100f;

    public override bool Fire()
    {
        if (!CanFire()) return false;

        Rigidbody rocket = rocketPool.GetProjectile();
        if (rocket == null) return false;

        rocket.transform.position = fireTransform.position;
        rocket.transform.rotation = fireTransform.rotation * Quaternion.Euler(90f, 0f, 0f);
        rocket.gameObject.SetActive(true);
        rocket.linearVelocity = launchForce * fireTransform.forward;

        cooldownTimer = 0f;
        return true;
    }
}
