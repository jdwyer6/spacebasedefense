using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] ETAClips;
    public string[] ETASubtitles;
    public GameObject subtitlesText;
    public bool playAudio = true;

    // Start is called before the first frame update
    void Start()
    {
        if(playAudio) {
            PlayRandomClip(ETAClips, ETASubtitles);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayRandomClip(AudioClip[] clips, string[] subtitles) {
        int randomidx = UnityEngine.Random.Range(0, clips.Length);
        source.clip = clips[randomidx];
        source.Play();
        subtitlesText.GetComponent<TextMeshProUGUI>().text = subtitles[randomidx];
        StartCoroutine(ShowSubtitles());
    }

    private IEnumerator ShowSubtitles() {
        subtitlesText.SetActive(true);
        yield return new WaitForSeconds(8);
        subtitlesText.SetActive(false);
    }
}
