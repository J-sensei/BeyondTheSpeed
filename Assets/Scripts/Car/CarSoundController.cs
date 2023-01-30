using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource EngineSound;
    [SerializeField] private AudioClip EngineBackFireSound;
    [SerializeField] private float EngineSoundOffset = 0.5f;
    private float initialEnginePitch;

    private CarController carController;
    private void Start()
    {
        carController = GetComponent<CarController>();
        initialEnginePitch = EngineSound.pitch; // Engine sound
        carController.BackFireAction += () => EngineSound.PlayOneShot(EngineBackFireSound);
    }

    private void Update()
    {
        // Slip Sound
        if (carController.CurrentMaxSlip > 0.2f)
        {
            if (!AudioManager.Instance.TyreSkid.isPlaying)
            {
                AudioManager.Instance.TyreSkid.Play();
            }
            float slipVolume = carController.CurrentMaxSlip / 1f;
            AudioManager.Instance.TyreSkid.volume = slipVolume * 1f;
            AudioManager.Instance.TyreSkid.pitch = Mathf.Clamp(slipVolume, 0.75f, 1);
        }
        else
        {
            AudioManager.Instance.TyreSkid.Stop();
        }

        EngineSound.pitch = (carController.EngineRPM / carController.MaxRPM) + EngineSoundOffset;

        if (carController.NitroBoost)
        {
            AudioManager.Instance.PlayLoop(AudioManager.Instance.NitroBoost);
        }
        else
        {
            AudioManager.Instance.NitroBoost.Stop();
        }
    }
}
