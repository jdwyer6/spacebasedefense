using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Juicer : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource impulseSourceHuge;
    
    private void Start() {
        
    }

    public IEnumerator ApplyHitStop(float seconds) {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(seconds);

        Time.timeScale = originalTimeScale;
    }

    public void ApplyCameraShake() {
        impulseSource.GenerateImpulse();
    }

    public void ApplyCameraShakeHuge() {
        impulseSourceHuge.GenerateImpulse();
    }
}
