using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Transform gun; // Objeto vacío que contiene las armas
 
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
            equippedWeapon.Fire();
        }
        if (Input.GetButtonUp("Fire1") && equippedWeapon is Flamethrower flamethrower)
        {
            flamethrower.StopFire();
        }
        
    }

    public void EquipWeapon(int index)
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
