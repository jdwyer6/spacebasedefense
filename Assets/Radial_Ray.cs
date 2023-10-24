using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radial_Ray : MonoBehaviour
{
    private bool rotate;
    public float rotationSpeed = 200f;
    public float timeBetweenRotations = 10;
    Transform originalParent;
    private float totalRotation = 0f; 
    public GameObject laser;
    private AudioManager am;
    public Collider2D col;
    

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        originalParent = transform.parent;
        StartCoroutine(InitiateRadialRay());
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate) {
            float rotationThisFrame = -rotationSpeed * Time.deltaTime;
            transform.SetParent(null);
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime, Space.World);
            totalRotation += Mathf.Abs(rotationThisFrame);
        }

        if(totalRotation >= 360f) {
            laser.SetActive(false);
            col.enabled = false;
            rotate = false;
            totalRotation = 0f; 
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.SetParent(originalParent);
        }

        if (transform.parent == null)
        {
            transform.position = originalParent.position;
        }
    }

    IEnumerator InitiateRadialRay() {
        while (true)
        {
            laser.SetActive(true);
            col.enabled = true;
            rotate = true;
            am.Play("RadialRay");
            yield return new WaitForSeconds(timeBetweenRotations);

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy_Health>().TakeDamage(50f);
        }
    }
}
