using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;
    private AudioManager am;
    private GameObject gm;
    private Data data;
    private Collider2D thisCollider;

    public enum IgnoreList
    {
        Player,
        Destructible_Environment,
        Enemy,
        Default
    }
    public IgnoreList ignoreList;

    private void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        Invoke("DestroySelf", 10f);
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();

        switch (ignoreList)
        {
            case IgnoreList.Enemy:
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    Physics2D.IgnoreCollision(thisCollider, enemy.GetComponent<Collider2D>(), true);
                }
                break;
            case IgnoreList.Player:
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player)
                    Physics2D.IgnoreCollision(thisCollider, player.GetComponent<Collider2D>(), true);
                break;
            case IgnoreList.Destructible_Environment:
                foreach (GameObject env in GameObject.FindGameObjectsWithTag("Destructible_Environment"))
                {
                    Physics2D.IgnoreCollision(thisCollider, env.GetComponent<Collider2D>(), true);
                }
                break;
            default:
                // Do nothing
                break;
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Destructible_Environment") {
            string clip_1 = data.wallImpactSounds[Random.Range(0, data.wallImpactSounds.Length)];
            string clip_2 = data.ricochetSounds[Random.Range(0, data.ricochetSounds.Length)];
            am.Play(clip_1);
            am.Play(clip_2);
        }

        if(other.gameObject.tag == "Destructible_Environment" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}
