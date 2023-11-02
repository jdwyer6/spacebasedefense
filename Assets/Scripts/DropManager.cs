using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public float timeActive = 20;
    private GameObject dropsContainer;
    private GameObject player;
    private Material enemyMaterial;
    private GameObject playerProjectile;
    public GameObject defaultProjectile;
    private GameObject gm;
    public GameObject duckProjectile;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GM");
        dropsContainer = GameObject.FindGameObjectWithTag("DropsUIContainer");
        player = GameObject.FindGameObjectWithTag("Player");
        playerProjectile = player.GetComponent<Player_Shooting>().projectilePrefab;
        player.GetComponent<Player_Shooting>().instakillActive = false;
        playerProjectile.GetComponent<Projectile_Homing>().isHoming = false;
        player.GetComponent<Player_Shooting>().laserActive = false;
        enemyMaterial = gm.GetComponent<Data>().enemyMaterial;
        playerProjectile.GetComponent<ChainReaction>().chainReactionEnabled = false;
        InitializeEnemyShader();
    }

    public void InitiateInstakill() {
        StartCoroutine(StartInstakill());
    }

    public IEnumerator StartInstakill() {
        player.GetComponent<Player_Shooting>().instakillActive = true;
        yield return new WaitForSeconds(timeActive);
        RemoveDropUI("instakill");
        player.GetComponent<Player_Shooting>().instakillActive = false;
    }

    // ----------------------------------------------------------------------

    public void InitiateHoming() {
        StartCoroutine(StartHoming());
    }

    public IEnumerator StartHoming() {
        playerProjectile = player.GetComponent<Player_Shooting>().projectilePrefab;
        playerProjectile.GetComponent<Projectile_Homing>().isHoming = true;
        yield return new WaitForSeconds(timeActive);
        playerProjectile.GetComponent<Projectile_Homing>().isHoming = false;
        RemoveDropUI("homing");
    }

    // ----------------------------------------------------------------------

    public void InitiateLaser() {
        StartCoroutine(StartLaser());
    }

    private IEnumerator StartLaser() {
        player.GetComponent<Player_Shooting>().laserActive = true;
        yield return new WaitForSeconds(timeActive);
        player.GetComponent<Player_Shooting>().laserActive = false;
        RemoveDropUI("laser");
    }

    // ----------------------------------------------------------------------

    public void InitiateDuck() {
        StartCoroutine(StartDucks());
    }

    private IEnumerator StartDucks() {
        player.GetComponent<Player_Shooting>().SetProjectile(duckProjectile);
        yield return new WaitForSeconds(timeActive);
        player.GetComponent<Player_Shooting>().SetProjectile(defaultProjectile);
        RemoveDropUI("duck");
    }

    // ----------------------------------------------------------------------

    public void InitiateArcticBlast() {
        StartCoroutine(StartArcticBlast());
    }

    private IEnumerator StartArcticBlast() {
        enemyMaterial.EnableKeyword("HSV_ON");
        enemyMaterial.EnableKeyword("OUTBASE_ON");
        enemyMaterial.SetFloat("_OutlineGlow", 50);
        enemyMaterial.SetFloat("(_OutlineAlpha", 0);
        enemyMaterial.EnableKeyword("OUTLINE_DISTORTION_ON");

        int timeToRemainFrozen = 10;
        for (int i = 0; i < timeToRemainFrozen; i++)
        {
            FreezeEnemyPos();
            yield return new WaitForSeconds(1);
        }

        InitializeEnemyShader(); // Undo shader
        foreach (var enemy in GetEnemies())
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None; // Remove position constraints
            if(enemy.GetComponent<Enemy_Shooting>() != null) {
                enemy.GetComponent<Enemy_Shooting>().enabled = true;
            }

            if(enemy.GetComponent<Enemy_Laser>() != null) {
                enemy.GetComponent<Enemy_Laser>().enabled = true;
            }

            if(enemy.GetComponent<Spiral_Shooting>() != null) {
                enemy.GetComponent<Spiral_Shooting>().enabled = true;
            }
        }
        RemoveDropUI("arcticBlast");
    }

    private void InitializeEnemyShader() {
        enemyMaterial.DisableKeyword("HSV_ON");
        enemyMaterial.DisableKeyword("OUTBASE_ON");
    }

    private void FreezeEnemyPos() {
        foreach (var enemy in GetEnemies())
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            if(enemy.GetComponent<Enemy_Shooting>() != null) {
                enemy.GetComponent<Enemy_Shooting>().enabled = false;
            }

            if(enemy.GetComponent<Enemy_Laser>() != null) {
                enemy.GetComponent<Enemy_Laser>().enabled = false;
            }

            if(enemy.GetComponent<Spiral_Shooting>() != null) {
                enemy.GetComponent<Spiral_Shooting>().enabled = false;
            }
            
        }
    }

    // ----------------------------------------------------------------------

    public void IniateChainReaction() {
        StartCoroutine(StartChainReaction());
    }

    private IEnumerator StartChainReaction() {
        playerProjectile = player.GetComponent<Player_Shooting>().projectilePrefab;
        playerProjectile.GetComponent<ChainReaction>().chainReactionEnabled = true;
        yield return new WaitForSeconds(timeActive);
        playerProjectile.GetComponent<ChainReaction>().chainReactionEnabled = false;
        RemoveDropUI("chainReaction");
    }

    // ----------------------------------------------------------------------

    private void RemoveDropUI(string code) {
        Transform childTransform = dropsContainer.transform.Find(code);
        if (childTransform != null) {
            childTransform.gameObject.SetActive(false);
        } else {
            Debug.LogWarning("Couldn't find " + code + " child in 'DropsUIContainer'.");
        }
    }

    private GameObject[] GetEnemies() {
        return GameObject.FindGameObjectsWithTag("Enemy");
    }
}
