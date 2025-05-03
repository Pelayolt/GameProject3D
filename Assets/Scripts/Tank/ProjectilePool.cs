using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public int poolSize = 20; // Aumenta si necesitas más lanzamientos simultáneos

    private List<Rigidbody> projectiles;

    void Awake()
    {
        projectiles = new List<Rigidbody>();

        for (int i = 0; i < poolSize; i++)
        {
            Rigidbody p = Instantiate(projectilePrefab, transform);
            p.gameObject.SetActive(false);
            projectiles.Add(p);
        }
    }

    public Rigidbody GetProjectile()
    {
        foreach (var p in projectiles)
        {
            if (!p.gameObject.activeInHierarchy)
                return p;
        }

        return null; // No hay disponibles
    }

    public List<Rigidbody> GetProjectiles(int count)
    {
        List<Rigidbody> available = new List<Rigidbody>();

        foreach (var p in projectiles)
        {
            if (!p.gameObject.activeInHierarchy)
            {
                available.Add(p);
                if (available.Count == count)
                    break;
            }
        }

        return (available.Count == count) ? available : null;
    }
}
