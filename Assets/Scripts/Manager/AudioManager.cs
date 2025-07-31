using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private VolumeData volumeData;
    [SerializeField] private SoundClipsSO soundClips;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private AudioSource musicAudioSource;
    private IObjectPool<AudioSource> pool;

    private void Start()
    {
        GameEventManager.Instance.AddListener<MusicSliderChangeEvent>(OnMusicSliderChange);
        GameEventManager.Instance.AddListener<SfxSliderChangeEvent>(OnSfxSliderChange);
        GameEventManager.Instance.AddListener<PlayMusicEvent>(PlayMusic);
        GameEventManager.Instance.AddListener<StopMusicEvent>(StopMusic);
        GameEventManager.Instance.AddListener<PauseMusicEvent>(PauseMusic);
        GameEventManager.Instance.AddListener<PlayAudioClipEvent>(PlayClip);
        GameEventManager.Instance.AddListener<PlayAudioClip3DEvent>(PlayClip3D);

        mixer.SetFloat("SfxVolume", Utils.LinearToDecibel(volumeData.Sfx));
        mixer.SetFloat("MusicVolume", Utils.LinearToDecibel(volumeData.Music));
        mixer.SetFloat("MasterVolume", Utils.LinearToDecibel(volumeData.Master));

        musicAudioSource = GetComponent<AudioSource>();
        pool = new ObjectPool<AudioSource>(
            CreateAudioSource, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);

        musicAudioSource.clip = soundClips.music;
        musicAudioSource.loop = true;
        musicAudioSource.Stop();
    }
    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<MusicSliderChangeEvent>(OnMusicSliderChange);
        GameEventManager.Instance.RemoveListener<SfxSliderChangeEvent>(OnSfxSliderChange);
        GameEventManager.Instance.RemoveListener<PlayMusicEvent>(PlayMusic);
        GameEventManager.Instance.RemoveListener<StopMusicEvent>(StopMusic);
        GameEventManager.Instance.RemoveListener<PauseMusicEvent>(PauseMusic);
        GameEventManager.Instance.RemoveListener<PlayAudioClipEvent>(PlayClip);
        GameEventManager.Instance.RemoveListener<PlayAudioClip3DEvent>(PlayClip3D);

        StopAllCoroutines();
        pool.Clear();
    }

    private void PlayMusic(GameEvent gameEvent)
    {
        musicAudioSource.Play();
    }

    private void PauseMusic(GameEvent gameEvent)
    {
        musicAudioSource.Pause();
    }

    private void StopMusic(GameEvent gameEvent)
    {
        musicAudioSource.Stop();
    }

    private void PlayClip(GameEvent gameEvent)
    {
        PlayAudioClipEvent playClipEvent = (PlayAudioClipEvent)gameEvent;

        AudioSource audioSource = pool.Get();
        audioSource.transform.position = Vector3.zero;
        audioSource.spatialBlend = 0.0f;
        audioSource.clip = playClipEvent.audioClip;
        audioSource.Play();
        StartCoroutine(ReleaseAudioSourceIfFinish(audioSource));
    }


    private void PlayClip3D(GameEvent gameEvent)
    {
        PlayAudioClip3DEvent playClipEvent = (PlayAudioClip3DEvent)gameEvent;

        AudioSource audioSource = pool.Get();
        audioSource.transform.position = playClipEvent.position;
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = playClipEvent.min;
        audioSource.maxDistance = playClipEvent.max;
        audioSource.clip = playClipEvent.audioClip;
        audioSource.Play();
        StartCoroutine(ReleaseAudioSourceIfFinish(audioSource));
    }

    private void OnSfxSliderChange(GameEvent gameEvent)
    {
        SfxSliderChangeEvent sliderChagneEvent = (SfxSliderChangeEvent)gameEvent;
        volumeData.Sfx = sliderChagneEvent.value;
        mixer.SetFloat("SfxVolume", Utils.LinearToDecibel(volumeData.Sfx));
    }

    private void OnMusicSliderChange(GameEvent gameEvent)
    {
        MusicSliderChangeEvent sliderChagneEvent = (MusicSliderChangeEvent)gameEvent;
        volumeData.Music = sliderChagneEvent.value;
        mixer.SetFloat("MusicVolume", Utils.LinearToDecibel(volumeData.Music));
    }

    private IEnumerator ReleaseAudioSourceIfFinish(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        pool.Release(audioSource);
    }

    private AudioSource CreateAudioSource()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
        return audioSource;
    }

    private void OnReleaseToPool(AudioSource pooledObject)
    {
        pooledObject.enabled = false;
        pooledObject.gameObject.SetActive(false);
    }

    private void OnGetFromPool(AudioSource pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
        pooledObject.enabled = true;
        pooledObject.Stop();
    }

    private void OnDestroyPooledObject(AudioSource pooledObject)
    {
        Destroy(pooledObject);
    }
}