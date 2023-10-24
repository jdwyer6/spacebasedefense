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
        public float minSpeedOffset = -1.0f;
    public float maxSpeedOffset = 1.0f;
    public GameObject coinParticles;
    private TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = GameObject.FindGameObjectWithTag("GM");
        am = FindObjectOfType<AudioManager>();
        coinText = GameObject.FindGameObjectWithTag("CashText").GetComponent<TextMeshProUGUI>();
        speed += Random.Range(minSpeedOffset, maxSpeedOffset);
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
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float easingFactor = 1.0f / (distanceToTarget + 1);
        float step = speed * easingFactor * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}
