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

    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        offset = transform.position - tankTurret.position;
    }

    void BlockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;      
    }

    void UnblockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void OnEnable()
    {
        BlockMouse();
        SetGunTransparency(true); // Transparencia alta al activar
    }

    void OnDisable()
    {
        UnblockMouse();
        SetGunTransparency(false);   // Totalmente opaco al desactivar
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            UnblockMouse();
            paused = true;
        }else{
            if(paused)
            {
                BlockMouse();
                paused = false;
            }
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            tankTurret.Rotate(Vector3.up * mouseX);
        }
        
    }

    void LateUpdate()
    {
        float pitch = tankGun.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        transform.position = tankTurret.position + tankTurret.rotation * offset;

        Quaternion verticalRot = Quaternion.Euler(pitch, 0f, 0f);
        Quaternion horizontalRot = Quaternion.Euler(0f, tankTurret.eulerAngles.y, 0f);
        transform.rotation = horizontalRot * verticalRot;
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
