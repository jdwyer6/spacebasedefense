using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Manager : MonoBehaviour
{
    private Enemy_Spawner spawner;
    bool waveTrackIsPlaying = true;
    bool coolDownTrackIsPlaying = false;
    private AudioManager am;
    private Data data;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Enemy_Spawner>();
        am = FindObjectOfType<AudioManager>();
        data = GetComponent<Data>();
        am.Play(data.waveSoundtrack);
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawner.waveActive && waveTrackIsPlaying) {
            waveTrackIsPlaying = false;
            am.Pause(data.waveSoundtrack);
            am.Play(data.coolDownSoundtrack);
            coolDownTrackIsPlaying = true;
        }

        if(spawner.waveActive && coolDownTrackIsPlaying) {
            coolDownTrackIsPlaying = false;
            am.Pause(data.coolDownSoundtrack);
            am.Play(data.waveSoundtrack);
            waveTrackIsPlaying = true;
        }
    }
}
