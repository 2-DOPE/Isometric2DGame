using UnityEngine;

public class CameraPanComponent : CharacterComponent
{
    [Header("References")]

    [InspectorName("Camera Follow Target"), Tooltip("Refrence the Game Object wich the camera uses to look around with in here. \n Note: This must be a child Object of this Game object (camera moves it around and follows it to be able to move away from player)")]
    [SerializeField] private GameObject followCameraTarget;

    [Header("Settings")]

    [InspectorName("Camera Pan Distance"), Tooltip("the maximum distance for camera to pan at.")]
    [SerializeField, Min(0)] private float panAmount = 3f;

    [InspectorName("Camera Pan Speed"), Tooltip("how fast the Camera moves while panning.")]
    [SerializeField, Min(0)] private float panSpeed = 5f;

    [InspectorName("Camera return Speed"), Tooltip("how fast the Camera moves when recentring.")]
    [SerializeField, Min(0)] private float returnSpeed = 3f;

    private Vector3 currentOffset;

    public void CameraLookAt(Vector2 lookInput)
    {
        Vector3 targetOffset = Vector3.zero;

        if (lookInput.sqrMagnitude > 0.001f)
        {
            Vector2 direction = lookInput.normalized;

            targetOffset = new Vector3(
                direction.x,
                direction.y,
                0f
            ) * panAmount;
        }

        float speed = targetOffset.sqrMagnitude > 0
            ? panSpeed
            : returnSpeed;

        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            speed * Time.deltaTime
        );

        followCameraTarget.transform.position =
            Owner.transform.position + currentOffset;
    }

    public void Reset()
    {
        currentOffset = Vector3.zero;
        followCameraTarget.transform.position = Owner.transform.position;
    }
}