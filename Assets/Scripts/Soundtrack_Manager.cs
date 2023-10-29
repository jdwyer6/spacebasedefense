using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Manager : MonoBehaviour
{
    private Enemy_Spawner spawner;
    private AudioManager am;
    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        data = GetComponent<Data>();
        audioSource.clip = data.soundtracks[currentSoundtrack];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if(am == null) Debug.LogError("AudioManager (am) is null");
        if(data == null) Debug.LogError("Data (data) is null");
        if(data.soundtracks == null) Debug.LogError("soundtracks array in Data is null");
        if(currentSoundtrack >= data.soundtracks.Length || data.soundtracks[currentSoundtrack] == null) 
            Debug.LogError("Current soundtrack string is out of bounds or null");


        if (HasClipFinishedPlaying(audioSource) && !incrememtingSoundtrack) {
            incrememtingSoundtrack = true;
            incrememtSoundtrack();
        }
    }

    public void incrememtSoundtrack() {
        if(incrememtingSoundtrack){
            currentSoundtrack += 1;
            audioSource.clip = data.soundtracks[currentSoundtrack];
            audioSource.Play();
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
