using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PickupHealing : MonoBehaviour
{
    public float healAmount = 25f;
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
        ResetPickup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;

        // Buscar componente de salud del tanque
        HealthSystem tankHealth = other.GetComponent<HealthSystem>();
        if (tankHealth != null && tankHealth.currentHealth < tankHealth.maxHealth)
        {
            tankHealth.Heal(healAmount); // Método que debés tener en tu clase TankHealth

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
        gameObject.SetActive(false);
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
