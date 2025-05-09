using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    public float damagePerHit = 5f;

    private void OnParticleCollision(GameObject other)
    {
        IDamageable dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damagePerHit);
        }
    }
}