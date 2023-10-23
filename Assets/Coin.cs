using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coin : MonoBehaviour
{
    private Transform player;
    private GameObject gm;
    private AudioManager am;
    public float speed = 5.0f;
    public GameObject coinParticles;
    private TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("GM");
        am = FindObjectOfType<AudioManager>();
        coinText = GameObject.FindGameObjectWithTag("CashText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            transform.position = nextPosition;
        }

        MoveTowardsTargetWithEasing(player.position);
    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            gm.GetComponent<Data>().playerPrefData.cash++;
            am.Play("Coin");
            Instantiate(coinParticles, transform.position, Quaternion.identity);
            coinText.text = gm.GetComponent<Data>().playerPrefData.cash.ToString();
            Destroy(gameObject);
        }
    }

    void MoveTowardsTargetWithEasing(Vector3 targetPosition)
    {
        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Calculate an easing factor based on distance, with an inverse square relationship
        // The +1 is to ensure we never divide by zero
        float easingFactor = 1.0f / (distanceToTarget + 1);

        // Calculate the step to move this frame
        float step = speed * easingFactor * Time.deltaTime;

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
