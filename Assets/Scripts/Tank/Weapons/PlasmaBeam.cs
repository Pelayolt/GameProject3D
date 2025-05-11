using System.Collections;
using UnityEngine;

public class PlasmaBeam : TankWeapon
{
    public GameObject laserEffect;       // Prefab del láser (de FX Lightning II)
    public float laserDuration = 3f;   // Tiempo visible del láser
    public override float cooldownTime => 6f;
    public GameObject laserEffect2;
    public AudioSource audioSource;
    public AudioClip fireClip;
    public bool isFiring = false;
    public float damagePerSecond = 20f;
    public float maxDistance = 1000f;
    private Vector3 laserStart;
    private Vector3 laserEnd;
    public GameObject impactAnimationPrefab;
    private bool impactShown = false;

    public LayerMask hitLayers;

    void OnEnable()
    {
        if (audioSource != null)
        {
            audioSource.clip = fireClip;
            audioSource.Stop();
        }
        laserEffect.SetActive(false);
        laserEffect2.SetActive(false);
        cooldownTimer = cooldownTime;
        isFiring = false;
    }

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
        impactShown = false;

        UpdateLaser();

        laserEffect.SetActive(true);
        laserEffect2.SetActive(true);

        float timeElapsed = 0f;

        while (timeElapsed < laserDuration)
        {
            UpdateLaser();
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        laserEffect.SetActive(false);
        laserEffect2.SetActive(false);
        isFiring = false;
    }

    private void UpdateLaser()
    {
        laserStart = fireTransform.position;
        Vector3 direction = fireTransform.forward;
        laserEnd = laserStart + direction * maxDistance;

        // Aplica daño continuo a todos los enemigos alcanzados
        RaycastHit[] hits = Physics.RaycastAll(laserStart, direction, maxDistance, hitLayers);
        foreach (RaycastHit hit in hits)
        {

            IDamageable dmg = hit.collider.GetComponent<IDamageable>();
            if (dmg == null)
                dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg == null)
                dmg = hit.collider.GetComponentInChildren<IDamageable>();   
            Debug.Log("PlasmaBeam: " + hit.collider.name);

            if (dmg != null)
            {
                float damage = damagePerSecond * Time.deltaTime;
                dmg.TakeDamage(damage);
            }

            // Visualmente termina en el primer impacto
            if (hit.distance < (laserEnd - laserStart).magnitude)
            {
                laserEnd = hit.point;
            }

            if (!impactShown && impactAnimationPrefab != null)
            {
                Instantiate(impactAnimationPrefab, hit.point, Quaternion.identity);
                impactShown = true;
            }
            
        }

        // Visuales del láser
        Vector3 dirToTarget = laserEnd - laserStart;
        float length = dirToTarget.magnitude;

        laserEffect.transform.position = laserStart + laserEffect.transform.up * (length / 2f);
        laserEffect.transform.localScale = new Vector3(1f, length, 1f);

        laserEffect2.transform.position = laserStart + laserEffect2.transform.up * (length / 2f);
        laserEffect2.transform.localScale = new Vector3(1f, length, 1f);
    }
}