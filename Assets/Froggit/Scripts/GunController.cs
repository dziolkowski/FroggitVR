using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    public Transform shootPoint;
    public BulletPool bulletPool;
    public float bulletSpeed = 20f;

    private int ammoCount = 6;
    private int shots = 0;
    private bool isBusy = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBusy && ammoCount > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = bulletPool.GetBullet();
        if (bullet == null) return;

        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        bullet.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 cameraForward = Camera.main.transform.forward;
        rb.velocity = cameraForward.normalized * bulletSpeed;

        ammoCount--;
        shots++;
        ScoreManager.Instance.UpdateAmmo(ammoCount);

        if (shots >= 6)
        {
            StartCoroutine(ScoreManager.Instance.ForceEnd());
        }

        isBusy = true;
        StartCoroutine(ResetBusy(0.5f));
    }

    IEnumerator ResetBusy(float delay)
    {
        yield return new WaitForSeconds(delay);
        isBusy = false;
    }
}
