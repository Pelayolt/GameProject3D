using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : TankWeapon
{
    public ProjectilePool rocketPool;
    public float launchForce = 100f;
    public AudioSource audioSource;
    public AudioClip fireClip;
    public Transform fireTransform1;
    public Transform fireTransform2;
    public Transform fireTransform3;
    public Transform fireTransform4;

    void OnEnable()
    {
        if (audioSource != null)
        {
            audioSource.clip = fireClip;
            audioSource.Stop();
        }
        cooldownTimer = cooldownTime;

    }

    public override void Fire()
    {
        if (!CanFire()) return;

        if (audioSource != null && fireClip != null)
        {
            audioSource.PlayOneShot(fireClip, 0.20f);
        }

        List<Rigidbody> rockets = rocketPool.GetProjectiles(4);
        if (rockets == null) return;

        Transform[] fireTransforms = new Transform[] { fireTransform1, fireTransform2, fireTransform3, fireTransform4 };

        StartCoroutine(FireRocketsSequentially(rockets, fireTransforms));
        cooldownTimer = 0f;
    }

    private IEnumerator FireRocketsSequentially(List<Rigidbody> rockets, Transform[] fireTransforms)
    {
        for (int i = 0; i < 4; i++)
        {
            Rigidbody rocket = rockets[i];
            Transform firePoint = fireTransforms[i];

            rocket.transform.position = firePoint.position;
            rocket.transform.rotation = firePoint.rotation * Quaternion.Euler(90f, 0f, 0f);
            rocket.gameObject.SetActive(true);
            rocket.linearVelocity = launchForce * firePoint.forward;

            yield return new WaitForSeconds(0.1f); // Espera 100ms
        }
    }
}
