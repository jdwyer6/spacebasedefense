using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Juicer : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    
    private void Start() {
        
    }

    public IEnumerator ApplyHitStop(int frames) {
        Time.timeScale = 0;

        for (int i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;
    }

    public void ApplyCameraShake() {
        impulseSource.GenerateImpulse();
    }
}
