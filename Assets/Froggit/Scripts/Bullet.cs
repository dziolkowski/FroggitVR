using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private bool hasScored = false;
    private Coroutine timeoutRoutine;

    void OnEnable()
    {
        // Uruchamiamy timeout przy aktywacji pocisku
        timeoutRoutine = StartCoroutine(Timeout());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasScored) return;

        if (collision.collider.CompareTag("RobotPart"))
        {
            string partName = collision.collider.name;
            int points = ScoreManager.Instance.GetPointsForPart(partName);
            ScoreManager.Instance.RegisterHit(points, partName);
            hasScored = true;

            if (timeoutRoutine != null)
                StopCoroutine(timeoutRoutine);

            DeactivateImmediately(); // znika od razu po trafieniu
        }
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(5f); // czas zycia pocisku przy pudle
        DeactivateImmediately();
    }

    void DeactivateImmediately()
    {
        hasScored = false;
        gameObject.SetActive(false);
        transform.position = ScoreManager.Instance.magazine.position;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
