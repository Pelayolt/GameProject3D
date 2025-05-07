using System.Collections;
using UnityEngine;

public class PlasmaBeam : TankWeapon
{
    public GameObject laserEffect;       // Prefab del láser (de FX Lightning II)
    public float laserDuration = 3f;   // Tiempo visible del láser
    public override float cooldownTime => 4f;
    public GameObject laserEffect2;
    public AudioSource audioSource;
    public AudioClip fireClip;
    private bool isFiring = false;

    public LayerMask hitLayers;

    public override void Fire()
    {
        if (!CanFire()) return;

        if (audioSource != null && fireClip != null)
        {
            audioSource.PlayOneShot(fireClip, 0.20f);
        }

        if (!isFiring)
        {
            StartCoroutine(FireLaser());
            cooldownTimer = 0f; // Reinicia el cooldown al disparar
        }
    }

    private IEnumerator FireLaser()
    {
        isFiring = true;

        // Parámetros
        Vector3 start = fireTransform.position;
        Vector3 direction = fireTransform.forward;
        float maxDistance = 1000f;

        RaycastHit hit;
        Vector3 endPoint;

        // Raycast: choca con algo o se extiende hasta maxDistance
        Physics.Raycast(start, direction, out hit, maxDistance, hitLayers);
        
        endPoint = hit.point;

            // Opcional: aplicar daño si tiene componente
            // hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
    

        // Calcular dirección y longitud
        Vector3 dirToTarget = endPoint - start;
        float length = dirToTarget.magnitude;

        // Mostrar y alinear el láser visualmente
        laserEffect.SetActive(true);
        laserEffect.transform.position = start + laserEffect.transform.up * (length / 2f);
        laserEffect.transform.localScale = new Vector3(1f, length, 1f); // ← se alarga solo lo necesario

        laserEffect2.SetActive(true);
        laserEffect2.transform.position = start + laserEffect2.transform.up * (length / 2f);
        laserEffect2.transform.localScale = new Vector3(1f, length, 1f); // ← se alarga solo lo necesario

        yield return new WaitForSeconds(laserDuration);
        laserEffect.SetActive(false);
        laserEffect2.SetActive(false);
        isFiring = false;
    }
}