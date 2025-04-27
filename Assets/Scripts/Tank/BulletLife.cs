using UnityEngine;

public class BulletLife : MonoBehaviour
{
    public float lifetime = 2f;
    public GameObject explosionPrefab; // Prefab de la explosión

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(DisableBullet), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Crear la explosión en el punto de impacto
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Desactivar la bala
        gameObject.SetActive(false);
    }

    private void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}