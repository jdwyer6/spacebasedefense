using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameObject environmentFull;
    public GameObject[] environmentDamaged;

    public Sprite blood;
    public string[] ricochetSounds;
    public string[] wallImpactSounds;
    public string[] playerDamageSounds;
    public string[] bloodHits;

    //Audio
    public string coolDownSoundtrack;
    public AudioClip[] soundtracks;

    //Particles
    public GameObject destructionParticles;
    public GameObject dustParticles;

    public GameObject[] enemies;
    public Upgrade[] upgrades;
    public Wave[] waves;
    public GameObject[] bossDrops;

    public Character[] characters;
}
