using UnityEngine;

public abstract class TankWeapon : MonoBehaviour
{
    public Transform fireTransform;
    public virtual float cooldownTime => 1f;
    protected float cooldownTimer;

    protected virtual void OnEnable()
    {
        cooldownTimer = cooldownTime;
    }

    public abstract void Fire();

    protected virtual void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public bool CanFire()
    {
        return cooldownTimer >= cooldownTime;
    }
}
