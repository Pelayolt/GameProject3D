using UnityEngine;

public class Flamethrower : TankWeapon
{
    public ParticleSystem flameEffect;

    private bool isFiring = false;

    public override bool Fire()
    {
        if (!CanFire()) return false;

        if (!isFiring)
        {
            flameEffect.Play();
            isFiring = true;
        }

        cooldownTimer = 0f;
        return true;
    }

    protected override void Update()
    {
        base.Update();

        if (isFiring && !Input.GetButton("Fire1"))
        {
            flameEffect.Stop();
            isFiring = false;
        }
    }
}
