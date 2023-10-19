using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniBulletHellHelper : MonoBehaviour
{
    private AudioManager am;
    private GameObject gm;
    bool soundPlayed = false;

    private int numTimesCalled = 0;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShotAuto1() {
        am.Play("Enemy_Shot_2");
        Debug.Log("Method Called" + numTimesCalled);
        numTimesCalled++;
    }

    public void PlaySound(string sound) {
        am.Play(sound);
    }

    public void PlaySoundOnce(string sound) {
        if(!soundPlayed) {
            soundPlayed = true;
            am.Play(sound);
            StartCoroutine(ResetSoundPlayed());
        }
        
    }

    IEnumerator ResetSoundPlayed() {
        yield return new WaitForSeconds(2);
        soundPlayed = false;
    }

    public void ShakeCamera() {
        gm.GetComponent<Juicer>().ApplyCameraShake();
    }
}
