using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Manager : MonoBehaviour
{
    private Enemy_Spawner spawner;
    bool waveTrackIsPlaying = true;
    bool coolDownTrackIsPlaying = false;
    private AudioManager am;
    [SerializeField] private GameObject amPrefab;
    private Data data;

    public int currentSoundtrack = 0;
    bool incrememtingSoundtrack = false;

    private void Awake() {
        am = FindObjectOfType<AudioManager>();
        
        if (am == null) {
            am = Instantiate(amPrefab).GetComponent<AudioManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<Enemy_Spawner>();
        data = GetComponent<Data>();
        am.Play(data.soundtracks[currentSoundtrack]);
    }

    // Update is called once per frame
    void Update()
    {

        //  if(am == null) Debug.LogError("AudioManager (am) is null");
        // if(data == null) Debug.LogError("Data (data) is null");
        // if(data.soundtracks == null) Debug.LogError("soundtracks array in Data is null");
        // if(currentSoundtrack >= data.soundtracks.Length || data.soundtracks[currentSoundtrack] == null) 
        //     Debug.LogError("Current soundtrack string is out of bounds or null");


        // if(!spawner.waveActive && waveTrackIsPlaying) {
        //     waveTrackIsPlaying = false;
        //     am.Pause(data.soundtracks[currentSoundtrack]);
        //     am.Play(data.coolDownSoundtrack);
        //     coolDownTrackIsPlaying = true;
        // }

        // if(spawner.waveActive && coolDownTrackIsPlaying) {
        //     coolDownTrackIsPlaying = false;
        //     am.Pause(data.coolDownSoundtrack);
        //     am.Play(data.soundtracks[currentSoundtrack]);
        //     waveTrackIsPlaying = true;
        // }

        // if (HasClipFinishedPlaying(am.GetAudioSource(data.soundtracks[currentSoundtrack])) && !incrememtingSoundtrack) {
        //     incrememtingSoundtrack = true;
        //     incrememtSoundtrack();
        // }
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
        if (audioSource == null) 
        {
            Debug.LogError("audioSource is null");
            return false;
        }

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
