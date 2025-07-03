using UnityEngine;

public class XRHolsterFollower : MonoBehaviour
{
    [Tooltip("Reference to the XR camera (usually Main Camera)")]
    public Transform xrCamera;

    [Tooltip("Vertical placement from floor (0) to head (1). 0.5 = hips.")]
    [Range(0f, 1f)] public float verticalFactor = 0.5f;

    private Vector3 initialLocalOffset; // Relative to camera rotation

    private void Start()
    {
        if (xrCamera == null)
        {
            Debug.LogError("XRHolsterFollower: Please assign the XR Camera.");
            enabled = false;
            return;
        }

        // Get camera's initial Y rotation at start
        float initialYaw = xrCamera.eulerAngles.y;
        Quaternion inverseYaw = Quaternion.Inverse(Quaternion.Euler(0, initialYaw, 0));

        // Store the offset relative to player's facing direction at start
        Vector3 offset = transform.position - xrCamera.position;
        offset.y = 0;
        initialLocalOffset = inverseYaw * offset;
    }

    private void LateUpdate()
    {
        if (xrCamera == null) return;

        // Get current yaw
        float currentYaw = xrCamera.eulerAngles.y;
        Quaternion yawRotation = Quaternion.Euler(0, currentYaw, 0);

        // Rotate original offset to match current player yaw
        Vector3 rotatedOffset = yawRotation * initialLocalOffset;

        // Vertical midpoint between ground and head
        float targetY = Mathf.Lerp(0f, xrCamera.position.y, verticalFactor);

        // Set position
        Vector3 newPos = new Vector3(
            xrCamera.position.x + rotatedOffset.x,
            targetY,
            xrCamera.position.z + rotatedOffset.z
        );
        transform.position = newPos;

        // Set rotation to match player yaw (as if parented)
        transform.rotation = yawRotation;
    }
}
