using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PickupWeapon : MonoBehaviour
{
    public string weaponName;
    public AudioClip pickupSound;

    private AudioSource audioSource;
    private bool pickedUp = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        ResetPickup(); // ← Restauramos cuando se reactiva
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;

        TankShooting weaponManager = other.GetComponent<TankShooting>();
        if (weaponManager != null)
        {
            Transform gun = weaponManager.gun;

            for (int i = 0; i < gun.childCount; i++)
            {
                Transform child = gun.GetChild(i);
                if (child.name == weaponName)
                {
                    weaponManager.EquipWeapon(i);
                    break;
                }
            }

            pickedUp = true;

            // Reproduce el sonido y oculta visualmente
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound, 0.5f);

            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
            foreach (var c in GetComponents<Collider>())
                c.enabled = false;

            StartCoroutine(DisableAfterSound());
        }
    }

    private IEnumerator DisableAfterSound()
    {
        float delay = pickupSound != null ? pickupSound.length : 0.1f;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // Se desactiva hasta que el respawner lo active
    }

    private void ResetPickup()
    {
        pickedUp = false;

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;
        foreach (var c in GetComponents<Collider>())
            c.enabled = true;
    }
}
