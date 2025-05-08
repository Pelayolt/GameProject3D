using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Transform healthBarCanvas;     // Asigna el canvas hijo del tanque
    public Image healthBarFill;           // Asigna la imagen de la barra

    private Camera mainCamera;

    void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;

        // Posiciona la barra arriba del tanque
        if (healthBarCanvas != null)
        {
            Vector3 offset = new Vector3(0, 2.5f, 0); // ajusta según el modelo
            healthBarCanvas.localPosition = offset;
        }
    }

    void Update()
    {
        // Test: daño con espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }

        // La barra mira a la cámara
        if (healthBarCanvas != null && mainCamera != null)
        {
            healthBarCanvas.LookAt(mainCamera.transform);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }
}
