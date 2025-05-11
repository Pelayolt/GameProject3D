using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    public float damagePerHit = 5f;

    private void OnParticleCollision(GameObject collision)
    {
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
        if (dmg == null)
            dmg = collision.gameObject.GetComponentInParent<IDamageable>();
        if (dmg == null)
            dmg = collision.gameObject.GetComponentInChildren<IDamageable>();   

        if (dmg != null)
        {
            dmg.TakeDamage(damagePerHit);
        }
    }
}