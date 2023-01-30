using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle common audio
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Source")]
    [Space]
    [SerializeField] private AudioSource GeneralAudioSource;
    [SerializeField] private AudioSource GearShiftSound;
    [SerializeField] private AudioSource NitroBoostSound;
    [SerializeField] private AudioSource NitroPressSound;
    [SerializeField] private AudioSource CrashSound;
    [SerializeField] private AudioSource TyreSkidSound;

    [Header("Audio Clips")]
    [Space]
    [SerializeField] private AudioClip CollectCoinSoundClip;
    [SerializeField] private AudioClip CameraSnapSoundClip;
    [SerializeField] private AudioClip AddFuelSoundClip;
    [SerializeField] private AudioClip LevelUpSoundClip;

    public AudioSource GearShift => GearShiftSound;
    public AudioSource NitroBoost => NitroBoostSound;
    public AudioSource NitroPress => NitroPressSound;
    public AudioSource Crash => CrashSound;
    public AudioSource TyreSkid => TyreSkidSound;
    public AudioClip CollectCoinClip => CollectCoinSoundClip;
    public AudioClip CameraSnapClip => CameraSnapSoundClip;
    public AudioClip AddFuelClip => AddFuelSoundClip;
    public AudioClip LevelUpClip => LevelUpSoundClip;

    public void PlayCrashSound(float kph)
    {
        if (!CrashSound.isPlaying)
        {
            CrashSound.pitch = 1f + kph / 300f;
            CrashSound.volume = kph / 300f * 0.9f;
            CrashSound.Play();
        }
    }

    public void PlayLoop(AudioSource audio)
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }

    public void PlayClip(AudioClip clip)
    {
        GeneralAudioSource.PlayOneShot(clip);
    }

    protected override void AwakeSingleton()
    {
        
    }
}
