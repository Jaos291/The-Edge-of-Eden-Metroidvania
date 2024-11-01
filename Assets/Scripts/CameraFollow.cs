using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 0.1f;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position + offset, cameraSpeed);
    }
}
