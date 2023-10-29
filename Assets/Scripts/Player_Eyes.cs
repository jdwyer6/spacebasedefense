using UnityEngine;
using System.Collections;

public class Player_Eyes : MonoBehaviour
{
    public GameObject leftPupil;
    public GameObject rightPupil;
    public GameObject leftEyeHighlight;
    public GameObject rightEyeHighlight;

    public GameObject leftEyeLid;
    public GameObject rightEyeLid;

    private GameObject nearestEnemy;
    private float blinkTime = 5f; // Time interval to blink

    private void Start()
    {
        if (leftEyeLid) leftEyeLid.SetActive(false);
        if (rightEyeLid) rightEyeLid.SetActive(false);
        StartCoroutine(Blink());
    }

    private void Update()
    {
        FindNearestEnemy();
        if (nearestEnemy != null)
        {
            LookAtEnemy(leftPupil, nearestEnemy.transform.position);
            LookAtEnemy(rightPupil, nearestEnemy.transform.position);
        }

        if (leftEyeHighlight)
        {
            leftEyeHighlight.transform.up = Vector3.up;
        }

        if (rightEyeHighlight)
        {
            rightEyeHighlight.transform.up = Vector3.up;
        }
    }

    private void LookAtEnemy(GameObject pupil, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.z = 0;
        pupil.transform.localPosition = direction.normalized * 0.1f; // 0.1f is the radius, adjust as necessary
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkTime);
            if (leftEyeLid) leftEyeLid.SetActive(true);
            if (rightEyeLid) rightEyeLid.SetActive(true);
            yield return new WaitForSeconds(0.1f); // Adjust as necessary
            if (leftEyeLid) leftEyeLid.SetActive(false);
            if (rightEyeLid) rightEyeLid.SetActive(false);
        }
    }
}
