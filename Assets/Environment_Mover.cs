using System.Collections;
using UnityEngine;

public class Environment_Mover : MonoBehaviour
{
    public float duration = 2.0f;  // Duration of the movement in seconds
    public Transform spriteTransformTop;  // Reference to the top side sprite's transform
    public Transform spriteTransformTopLeft;  // Reference to the top left side sprite's transform
    public Transform spriteTransformTopRight;  // Reference to the top right side sprite's transform

    public Vector3 moveDirectionTop = Vector3.up;  // Direction in which the top side will move
    public Vector3 moveDirectionTopLeft = Vector3.up;  // Direction in which the top left side will move
    public Vector3 moveDirectionTopRight = Vector3.up;  // Direction in which the top right side will move

    private Vector3 initialPositionTop;  // Initial position of the top side sprite
    private Vector3 initialPositionTopLeft;  // Initial position of the top left side sprite
    private Vector3 initialPositionTopRight;  // Initial position of the top right side sprite

    private Vector3 targetPositionTop;  // Target position of the top side sprite
    private Vector3 targetPositionTopLeft;  // Target position of the top left side sprite
    private Vector3 targetPositionTopRight;  // Target position of the top right side sprite

    public bool chestItemCollected = false;
    private bool moveInitiated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteTransformTop == null || spriteTransformTopLeft == null || spriteTransformTopRight == null)
        {
            Debug.LogError("Sprite Transforms are not assigned. Please assign them in the Inspector.");
            enabled = false;  // Disable the script
            return;
        }

        chestItemCollected = false;

        initialPositionTop = spriteTransformTop.localPosition;
        initialPositionTopLeft = spriteTransformTopLeft.localPosition;
        initialPositionTopRight = spriteTransformTopRight.localPosition;

        targetPositionTop = initialPositionTop + moveDirectionTop;
        targetPositionTopLeft = initialPositionTopLeft + moveDirectionTopLeft;
        targetPositionTopRight = initialPositionTopRight + moveDirectionTopRight;
    }

    private void Update()
    {
        if (chestItemCollected && !moveInitiated)
        {
            StartCoroutine(MoveSides());
            moveInitiated = true;
        }
    }

    public IEnumerator MoveSides()
    {
        float elapsed = 0f;
        Debug.Log("Coroutine Called");

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Move the top side
            spriteTransformTop.localPosition = Vector3.Lerp(initialPositionTop, targetPositionTop, t);

            // Move the top left side
            spriteTransformTopLeft.localPosition = Vector3.Lerp(initialPositionTopLeft, targetPositionTopLeft, t);

            // Move the top right side
            spriteTransformTopRight.localPosition = Vector3.Lerp(initialPositionTopRight, targetPositionTopRight, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure all sides end at their target positions
        spriteTransformTop.localPosition = targetPositionTop;
        spriteTransformTopLeft.localPosition = targetPositionTopLeft;
        spriteTransformTopRight.localPosition = targetPositionTopRight;
    }
}
