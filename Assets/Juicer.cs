using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juicer : MonoBehaviour
{
    public IEnumerator ApplyHitStop(int frames) {
        Time.timeScale = 0;

        for (int i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;
    }
}
