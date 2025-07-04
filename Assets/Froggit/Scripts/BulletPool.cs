using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();
    public Transform magazine;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, magazine.position, Quaternion.identity);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject b in pool)
        {
            if (!b.activeInHierarchy)
                return b;
        }
        return null; // brak wolnych pociskow
    }
}
