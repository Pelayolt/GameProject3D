using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Transform gun; // Objeto vacío que contiene las armas
    public AudioSource audioSource;
    public AudioClip fireClip;

    public TankWeapon equippedWeapon;

    void Start()
    {
        EquipWeapon(0); // Equipa la primera arma al iniciar
    }

    void Update()
    {
        if(Time.deltaTime == 0)
            return;
        if (Input.GetButtonDown("Fire1") && equippedWeapon != null)
        {
            bool fired = equippedWeapon.Fire();
            if (fired && audioSource != null && fireClip != null)
            {
                audioSource.PlayOneShot(fireClip);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0); // Equipa la primera arma
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1); // Equipa la segunda arma
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2); // Equipa la segunda arma
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeapon(3); // Equipa el tercer arma
        }
    }

    void EquipWeapon(int index)
    {
        // Desactiva todas las armas
        for (int i = 0; i < gun.childCount; i++)
        {
            gun.GetChild(i).gameObject.SetActive(false);
        }

        // Activa el arma seleccionada si existe
        if (index >= 0 && index < gun.childCount)
        {
            Transform selectedWeapon = gun.GetChild(index);
            selectedWeapon.gameObject.SetActive(true);
            equippedWeapon = selectedWeapon.GetComponent<TankWeapon>();
        }
        else
        {
            equippedWeapon = null;
        }
    }
}
