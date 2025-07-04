using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 10f;
    private float timer;

    private bool hasScored = false;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ResetBullet();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasScored) return;

        if (collision.collider.CompareTag("RobotPart"))
        {
            string partName = collision.collider.name;
            int points = ScoreManager.Instance.GetPointsForPart(partName);
            ScoreManager.Instance.RegisterHit(points, partName);
        }

        ResetBullet();
    }

    void ResetBullet()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = ScoreManager.Instance.magazine.position;
        gameObject.SetActive(false);
    }
}
