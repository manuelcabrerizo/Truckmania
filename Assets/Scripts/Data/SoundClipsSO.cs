using UnityEngine;

[CreateAssetMenu(fileName = "SoundClipsSO", menuName = "SoundClipsSO/Data", order = 1)]

public class SoundClipsSO : ScriptableObject
{
    public AudioClip music;
    public AudioClip engine;
    public AudioClip drift;
    public AudioClip coin;
    public AudioClip box;
    public AudioClip explosion;

    public AudioClip barrilPickup;
    public AudioClip barrilShoot;

    public AudioClip mounsterIdle;
    public AudioClip mounsterHit;
    public AudioClip mounsterAttack;
    public AudioClip mounsterDeath;

    public AudioClip countDown;
    public AudioClip reset;
}
