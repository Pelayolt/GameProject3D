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
        SetGunTransparency(0.4f); // Transparencia alta al activar
    }

    void OnDisable()
    {
        UnblockMouse();
        SetGunTransparency(1f);   // Totalmente opaco al desactivar
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

    void SetGunTransparency(float alpha)
    {
        if (tankGun == null) return;

        foreach (Transform weapon in tankGun)
        {
            bool wasInactive = !weapon.gameObject.activeSelf;

            // Activamos temporalmente si estaba inactiva
            if (wasInactive)
                weapon.gameObject.SetActive(true);

            Renderer renderer = weapon.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (!mat.HasProperty("_Color")) continue;

                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;

                    if (alpha < 1f)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                    else
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.DisableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = -1;
                    }
                }
            }

            // Restaurar el estado de activación original
            if (wasInactive)
                weapon.gameObject.SetActive(false);
        }
    }


}
