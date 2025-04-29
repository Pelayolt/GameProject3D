using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public TankWeapon equippedWeapon;      // El arma equipada (script)
    public GameObject tankCannonModel;      // Modelo visual del cañón normal
    public GameObject rocketLauncherModel;  // Modelo visual del lanzacohetes

    public AudioSource audioSource;         // 🎶 Nuevo: fuente de audio para disparo
    public AudioClip fireClip;

    void Start()
    {
        EquipCannon(); // 🔥 Forzamos que al empezar solo esté activo el cañón
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (equippedWeapon != null)
            {
                bool fired = equippedWeapon.Fire();

                if (fired && audioSource != null && fireClip != null)
                {
                    audioSource.PlayOneShot(fireClip);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipCannon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipRocketLauncher();
        }
    }

    void EquipCannon()
    {
        tankCannonModel.SetActive(true);
        rocketLauncherModel.SetActive(false);
        equippedWeapon = tankCannonModel.GetComponent<TankWeapon>();
    }

    void EquipRocketLauncher()
    {
        tankCannonModel.SetActive(false);
        rocketLauncherModel.SetActive(true);
        equippedWeapon = rocketLauncherModel.GetComponent<TankWeapon>();
    }
}