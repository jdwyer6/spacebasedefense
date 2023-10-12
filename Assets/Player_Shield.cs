using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shield : MonoBehaviour
{
    bool shieldActive = false;
    public GameObject shieldPrefab;
    private Player_Health playerHealth;
    private Player_Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<Player_Health>();
    }

    // Update is called once per frame
    void Update()
    {
        // float currentMoveSpeed = playerMovement.currentSpeed;
        if(Input.GetKey(KeyCode.LeftShift)) {
            shieldActive = true;
            shieldPrefab.SetActive(true);
            playerHealth.canTakeDamage = false;
            // playerMovement.moveSpeed = 0;
        } else {
            shieldActive = false;
            shieldPrefab.SetActive(false);
            playerHealth.canTakeDamage = true;
            // playerMovement.moveSpeed = playerMovement.currentMoveSpeed;
        }
    }
}
