using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital_Assassin : MonoBehaviour
{
[Tooltip("GameObject that will orbit around this object.")]
    public GameObject objectToOrbit;

    [Tooltip("Speed of the orbiting movement.")]
    public float orbitSpeed = 10.0f;

    [Tooltip("Distance from this object to the orbiting object.")]
    public float orbitDistance = 5.0f;

    private void Update()
    {
        if (objectToOrbit != null)
        {
            // Calculate the position for the object to orbit
            objectToOrbit.transform.RotateAround(transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);

            Vector3 desiredPosition = (objectToOrbit.transform.position - transform.position).normalized * orbitDistance;
            objectToOrbit.transform.position = new Vector3(desiredPosition.x, objectToOrbit.transform.position.y, desiredPosition.z);
        }
    }
}
