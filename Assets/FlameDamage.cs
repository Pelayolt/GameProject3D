using System.Collections.Generic;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public LayerMask enemyLayer;

    private List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();
    private bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;
        enemiesInRange.RemoveAll(e => e == null); // Limpia referencias rotas
    }

    private void Update()
    {
        if (!isActive) return;

        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
            enemiesInRange.Remove(enemy);
    }
}
