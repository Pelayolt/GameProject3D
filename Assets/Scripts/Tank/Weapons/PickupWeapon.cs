using UnityEngine;

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

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;
        pickedUp = true;

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

            // Reproduce el sonido y oculta visualmente
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound, 0.50f);

            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
            foreach (var c in GetComponents<Collider>())
                c.enabled = false;

            Destroy(gameObject, pickupSound != null ? pickupSound.length : 0.1f);
        }
    }
}
