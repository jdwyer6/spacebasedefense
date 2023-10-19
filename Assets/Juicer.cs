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

    public void Pulse(GameObject objectToPulse, float pulseSpeed = 1.0f, float pulseMagnitude = .1f) {
        StartCoroutine(InitializePulse(objectToPulse, pulseSpeed, pulseMagnitude));
    }

    private IEnumerator InitializePulse(GameObject objectToPulse, float pulseSpeed, float pulseMagnitude)
    {
        Vector3 initialScale = objectToPulse.transform.localScale;
        while (true)
        {
            float scaleChange = (Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude) + 1; // Adding 1 ensures we oscillate around the original scale
            objectToPulse.transform.localScale = initialScale * scaleChange;
            yield return null;
        }
    }
}
