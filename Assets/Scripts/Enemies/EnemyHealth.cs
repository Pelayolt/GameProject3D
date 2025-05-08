using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject deathVFX;

    [Header("Parpadeo al recibir daño")]
    public Renderer[] renderers;         // Asigna los Renderers del tanque
    public Color flashColor = Color.red; // Color de parpadeo
    public float flashDuration = 0.1f;   // Tiempo que dura el parpadeo

    private Color[] originalColors;

    void Awake()
    {
        currentHealth = maxHealth;

        // Guardar colores originales
        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].material.color;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        FlashDamage(); // 👈 Parpadeo al recibir daño

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathVFX)
            Instantiate(deathVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void FlashDamage()
    {
        if (renderers != null && renderers.Length > 0)
        {
            StopAllCoroutines(); // cancelar si ya estaba parpadeando
            StartCoroutine(FlashCoroutine());
        }
    }

    System.Collections.IEnumerator FlashCoroutine()
    {
        // Cambiar a color de flash
        foreach (var rend in renderers)
        {
            rend.material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restaurar color original
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}