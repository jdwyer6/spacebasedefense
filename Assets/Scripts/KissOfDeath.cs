using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KissOfDeath : MonoBehaviour
{
    public ParticleSystem[] particles;
    public Collider2D col;
    bool canFire;
    private AudioManager am;
    private GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;
        canFire = true;
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && canFire) {
            Fire();
        }
    }

    public void Fire() {
        am.Play("KissOfDeath");
        gm.GetComponent<Juicer>().ApplyCameraShakeHuge();
        foreach (var particle in particles)
        {
            particle.Play();
        }
        StartCoroutine(StartTimers());

    }

    IEnumerator StartTimers() {
        canFire = false;
        col.enabled = true;
        yield return new WaitForSeconds(.1f);
        col.enabled = false;
        yield return new WaitForSeconds(2f);
        canFire = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy_Health>().TakeDamage(50);
        }
    }
}
