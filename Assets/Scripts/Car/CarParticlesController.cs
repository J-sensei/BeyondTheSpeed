using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller to render the car particles
/// </summary>
public class CarParticlesController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> BackFireParticles;
    [SerializeField] private List<ParticleSystem> NitroParticles;
    [SerializeField] private ParticleSystem DriftTextParticles;
    [SerializeField] private ParticleSystem NitroTextParticle;

    //private GameManager gameManager;
    private CarController carController;
    private GameObject headLights;

    private void Start()
    {
        carController = GetComponent<CarController>();
        headLights = carController.transform.Find("Head Lights").gameObject;
        //gameManager = GameObject.Find(GameObjectNames.GAME_MANAGER).GetComponent<GameManager>();

        // Add particles to back fire actions 
        foreach (var particles in BackFireParticles)
        {
            carController.BackFireAction += () => particles.Emit(2);
        }

        foreach (var particles in NitroParticles)
        {
            carController.NitroBackFireAction += () => particles.Emit(2);
        }
    }


    private void Update()
    {
        // Render collision particles
        if (carController.Colliding && carController.KPH > 5f)
        {
            EffectManager.Instance.PlayParticle(EffectManager.Instance.CollisionSpark, carController.CollisionPosition);
        }
        else
        {
            EffectManager.Instance.CollisionSpark.Stop();
        }

        //if (carController.NitroBoost)
        //{
        //    EffectManager.Instance.SpeedLine.Emit(1);
        //}
        //else
        //{
        //    EffectManager.Instance.SpeedLine.Stop();
        //}

        // Nitro Text Particles
        if (carController.NitroBoost && !NitroTextParticle.isPlaying)
        {
            NitroTextParticle.Play();
        }
        else
        {
            NitroTextParticle.Stop();
        }

        // Head Light
        //float dayTime = gameManager.LightingManager.CurrentTime;
        //if((dayTime >= 0 && dayTime <= 5.5f) || (dayTime >= 18.5f && dayTime <= 24f))
        //{
        //    headLights.SetActive(true);
        //}
        //else
        //{
        //    headLights.SetActive(false);
        //}
    }

    public void PlayDriftParticle()
    {
        if (carController.IsDrifting && !DriftTextParticles.isPlaying)
        {
            DriftTextParticles.Play();
        }
        else
        {
            DriftTextParticles.Stop();
        }
    }
}
