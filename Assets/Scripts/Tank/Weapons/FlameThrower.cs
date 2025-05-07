using UnityEngine;

public class Flamethrower : TankWeapon
{
    public ParticleSystem flameEffect;
    public FlameDamage flameDamage;
    public AudioSource audioSource;
    public AudioClip fireClip;

    private bool isFiring = false;

    void Start()
    {
        flameEffect.Stop();
        flameDamage.SetActive(false);

        if (audioSource != null)
        {
            audioSource.clip = fireClip;
            audioSource.loop = true;
            audioSource.volume = 0.2f; // Aquí bajas el volumen
        }
    }

    public override void Fire()
    {
        if (!CanFire()) return;

        if (!isFiring)
        {
            flameEffect.Play();
            flameDamage.SetActive(true);
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
            flameDamage.SetActive(false);
            isFiring = false;

            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
