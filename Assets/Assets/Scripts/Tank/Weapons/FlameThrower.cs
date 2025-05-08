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
        if (!CanFire()) return;

        if (!isFiring)
        {
            flameEffect.Play();
            isFiring = true;

            if (audioSource != null && fireClip != null)
                audioSource.Play();
        }

        cooldownTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();

        if (isFiring && !Input.GetButton("Fire1"))
        {
            flameEffect.Stop();
            isFiring = false;

            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
