using UnityEngine;

public class Eyes_Follow_Mouse : MonoBehaviour
{
    public Transform leftPupil; // Assign left pupil in the inspector.
    public Transform rightPupil; // Assign right pupil in the inspector.
    public float followSpeed = 5f; // Speed at which the pupil follows the mouse.
    public float maxDistanceFromCenter = 0.1f; // Maximum distance pupil can move from its original position.

    private Vector3 leftPupilStartPosition;
    private Vector3 rightPupilStartPosition;

    void Start()
    {
        // Save the initial position of the pupils.
        leftPupilStartPosition = leftPupil.position;
        rightPupilStartPosition = rightPupil.position;
    }

    void Update()
    {
        // Get the mouse position in world space.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        
        // Direction from pupils to the mouse position.
        Vector3 leftDirection = (mousePos - leftPupilStartPosition).normalized;
        Vector3 rightDirection = (mousePos - rightPupilStartPosition).normalized;

        // Calculate the new pupil positions.
        Vector3 newLeftPupilPosition = leftPupilStartPosition + leftDirection * maxDistanceFromCenter;
        Vector3 newRightPupilPosition = rightPupilStartPosition + rightDirection * maxDistanceFromCenter;

        // Move pupils towards the target positions.
        leftPupil.position = Vector3.Lerp(leftPupil.position, newLeftPupilPosition, followSpeed * Time.deltaTime);
        rightPupil.position = Vector3.Lerp(rightPupil.position, newRightPupilPosition, followSpeed * Time.deltaTime);
    }
}
