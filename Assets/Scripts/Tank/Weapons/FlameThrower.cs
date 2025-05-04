using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : TankWeapon
{
    public ParticleSystem flameEffect;
    public Collider flameArea; // Trigger
    public float damagePerSecond = 10f;

    private List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();
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

        if (isFiring)
        {
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                    enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }

            // Si se deja de mantener presionado el disparo
            if (!Input.GetButton("Fire1"))
                StopFiring();
        }
    }

    private void StopFiring()
    {
        flameEffect.Stop();
        isFiring = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
            enemiesInRange.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null && enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);
    }
}
