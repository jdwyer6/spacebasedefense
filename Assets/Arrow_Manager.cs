using UnityEngine;
using UnityEngine.UI;

public class Arrow_Manager : MonoBehaviour
{
    public Image arrowPrefab; // Drag and drop the UI Image prefab (arrow) here in the inspector
    private Image arrowInstance; // Instance of the arrow in the scene
    public Canvas canvas; // Drag and drop the canvas you want the arrow to be a child of

    private Transform playerTransform;
    private GameObject nearestEnemy;

    private Camera mainCamera;
    private float minDistanceToShowArrow = 20f;

    private void Start()
    {
        playerTransform = this.transform; // Assuming this script is attached to the player
        mainCamera = Camera.main;

        // Create the arrow instance but set it to inactive initially
        arrowInstance = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        arrowInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        FindNearestEnemy();

        if (nearestEnemy != null)
        {
            float distanceToNearestEnemy = Vector3.Distance(playerTransform.position, nearestEnemy.transform.position);
            if (distanceToNearestEnemy > minDistanceToShowArrow)
            {
                if (!arrowInstance.gameObject.activeInHierarchy)
                    arrowInstance.gameObject.SetActive(true);

                PointArrowToEnemy();
            }
            else
            {
                if (arrowInstance.gameObject.activeInHierarchy)
                    arrowInstance.gameObject.SetActive(false);
            }
        }
        else
        {
            if (arrowInstance.gameObject.activeInHierarchy)
                arrowInstance.gameObject.SetActive(false);
        }
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
    }

    private void PointArrowToEnemy()
    {
        Vector3 directionToEnemy = nearestEnemy.transform.position - playerTransform.position;
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg - 90f; // Subtract 90 to correct for initial arrow rotation
        arrowInstance.transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 arrowPositionOnScreen = mainCamera.WorldToScreenPoint(playerTransform.position);
        arrowInstance.transform.position = arrowPositionOnScreen;
    }
}
