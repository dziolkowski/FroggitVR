using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private bool hasScored = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasScored) return;

        if (collision.collider.CompareTag("RobotPart"))
        {
            string partName = collision.collider.name;
            int points = ScoreManager.Instance.GetPointsForPart(partName);
            ScoreManager.Instance.RegisterHit(points, partName);
            hasScored = true;
        }

        StartCoroutine(DeactivateAfterSeconds(1f));
    }

    IEnumerator DeactivateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Reset bullet
        hasScored = false;
        gameObject.SetActive(false);
        transform.position = ScoreManager.Instance.magazine.position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
