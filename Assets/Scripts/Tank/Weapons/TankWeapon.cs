using UnityEngine;

public abstract class TankWeapon : MonoBehaviour
{
    public Transform fireTransform;
    public float cooldownTime = 1f;
    protected float cooldownTimer = 0f;

    public abstract bool Fire();

    protected virtual void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public bool CanFire()
    {
        return cooldownTimer >= cooldownTime;
    }
}
