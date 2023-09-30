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

    public int currentSoundtrack = 0;
    bool incrememtingSoundtrack = false;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Enemy_Spawner>();
        am = FindObjectOfType<AudioManager>();
        data = GetComponent<Data>();
        am.Play(data.soundtracks[currentSoundtrack]);
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawner.waveActive && waveTrackIsPlaying) {
            waveTrackIsPlaying = false;
            am.Pause(data.soundtracks[currentSoundtrack]);
            am.Play(data.coolDownSoundtrack);
            coolDownTrackIsPlaying = true;
        }

        if(spawner.waveActive && coolDownTrackIsPlaying) {
            coolDownTrackIsPlaying = false;
            am.Pause(data.coolDownSoundtrack);
            am.Play(data.soundtracks[currentSoundtrack]);
            waveTrackIsPlaying = true;
        }

        if (HasClipFinishedPlaying(am.GetAudioSource(data.soundtracks[currentSoundtrack])) && !incrememtingSoundtrack) {
            incrememtingSoundtrack = true;
            incrememtSoundtrack();
        }
    }

    public void incrememtSoundtrack() {
        if(incrememtingSoundtrack){
            currentSoundtrack += 1;
            am.Play(data.soundtracks[currentSoundtrack]);  
            incrememtingSoundtrack = false;
        }
    }

    bool HasClipFinishedPlaying(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {   
            return false;  // Still playing
        }
        else if (audioSource.time >= audioSource.clip.length)
        {
            return true;  // Clip has finished playing
        }
        return false;  // AudioSource is not playing, but the clip wasn't near the end.
    }
}
