using UnityEngine;

public class Flamethrower : TankWeapon
{
    public ParticleSystem flameEffect;
    public AudioSource audioSource;
    public AudioClip fireClip;
    public override float cooldownTime => 0f;

    private bool isFiring = false;

    void OnEnable()
    {
        flameEffect.Stop();
        isFiring = false;
        cooldownTimer = cooldownTime;

        if (audioSource != null)
        {
            audioSource.clip = fireClip;
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.Stop();
        }
    }

    public override void Fire()
    {
        if (!CanFire() || isFiring) return;
        flameEffect.Play();
        isFiring = true;

        if (audioSource != null && fireClip != null)
            audioSource.Play();
    }

    public void StopFire()
    {
        if (isFiring)
        {
            flameEffect.Stop();
            isFiring = false;

            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }

}
