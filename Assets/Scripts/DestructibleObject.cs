using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask projectileLayer;

    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si el objeto que colisionó está en la capa de proyectiles
        if (((1 << collision.gameObject.layer) & projectileLayer) != 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}