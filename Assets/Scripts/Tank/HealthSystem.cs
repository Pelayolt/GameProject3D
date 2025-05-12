using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public Image healthBarFill;
    public float maxHealth = 100f;
    public float currentHealth;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;

    public static string lastLevel;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            lastLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameOver");
            //Respawn();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Respawn()
    {
        // Resetear posici�n y rotaci�n
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Resetear velocidades si tiene Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(10f);
        }
    }
}
