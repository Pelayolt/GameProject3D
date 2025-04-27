using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public Rigidbody bulletPrefab;
    public int poolSize = 10; // Ajustar si en algún momento hay un arma que dispare más rápido

    private List<Rigidbody> bullets;

    void Awake()
    {
        Instance = this;
        bullets = new List<Rigidbody>();

        for (int i = 0; i < poolSize; i++)
        {
            Rigidbody bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bullets.Add(bullet);
        }
    }

    public Rigidbody GetBullet()
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }
}
