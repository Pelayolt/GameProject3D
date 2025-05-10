using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowFirstPerson : MonoBehaviour
{
    [Header("References")]
    public Transform tankTurret;
    public Transform tankGun;

    [Header("Settings")]
    public float mouseSensitivity = 2f;
    private bool paused = false;

    private Camera cam;
    private float pitch = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetGunTransparency(true); // Transparencia alta al activar
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetGunTransparency(false);   // Totalmente opaco al desactivar
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
        }else{
            if(paused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                paused = false;
            }
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            tankTurret.Rotate(Vector3.up * mouseX);
        }
    }

    void LateUpdate()
    {
        float gunPitch = tankGun.localEulerAngles.x;
        if (gunPitch > 180f) gunPitch -= 360f;

        transform.localRotation = Quaternion.Euler(gunPitch, 0f, 0f);
    }

    void SetGunTransparency(bool enableTransparent)
    {
        if (tankGun == null) return;

        foreach (Transform weapon in tankGun)
        {
            bool wasInactive = !weapon.gameObject.activeSelf;
            if (wasInactive)
                weapon.gameObject.SetActive(true); // Activar temporalmente

            Renderer renderer = weapon.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] mats = renderer.sharedMaterials;
                Material[] newMats = new Material[mats.Length];

                for (int i = 0; i < mats.Length; i++)
                {
                    string baseName = mats[i].name.Replace(" (Instance)", ""); // Quitar sufijo de instancia
                    string targetName = enableTransparent ? baseName + "_Transparent" : baseName.Replace("_Transparent", "");

                    // Cargar material desde Resources (debe estar en carpeta "Resources")
                    Material newMat = Resources.Load<Material>("Materials/" + targetName);
                    if (newMat != null)
                    {
                        newMats[i] = newMat;
                    }
                    else
                    {
                        Debug.LogWarning($"Material '{targetName}' no encontrado en Resources.");
                        newMats[i] = mats[i]; // Fallback al actual
                    }
                }

                renderer.sharedMaterials = newMats;
            }

            if (wasInactive)
                weapon.gameObject.SetActive(false); // Restaurar estado
        }
    }
}
