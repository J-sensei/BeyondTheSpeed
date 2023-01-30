using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the common particles and effect of the game
/// </summary>
public class EffectManager : Singleton<EffectManager>
{
    [Header("Particles")]
    [Tooltip("Smoke to render when the tyre is slippy")]
    [SerializeField] ParticleSystem TyreSmokeParticles;
	[SerializeField] ParticleSystem BoomTextParticle;
	[SerializeField] ParticleSystem CollisionSparkParticle;
	[SerializeField] ParticleSystem SpeedParticles;

    [Header("Trails")]
    [Tooltip("Trail ref, The lifetime of the tracks is configured in it")]
    [SerializeField] TrailRenderer TrailRef;
    [Tooltip("Parent for copy of TrailRef")]
    [SerializeField] Transform TrailsHolder;

	Queue<TrailRenderer> FreeTrails = new Queue<TrailRenderer>();
	public ParticleSystem TyreSmoke { get { return TyreSmokeParticles; } }
	public ParticleSystem BoomText { get { return BoomTextParticle; } }
	public ParticleSystem CollisionSpark { get { return CollisionSparkParticle; } }
	public ParticleSystem SpeedLine { get { return SpeedParticles; } }

	public void PlayParticle(ParticleSystem particle, Vector3 position)
    {
		Quaternion quaternion = Quaternion.identity;
		particle.transform.position = position;
		particle.transform.rotation = quaternion;
		particle.Emit(1);
	}

	public TrailRenderer GetTrail(Vector3 startPos)
	{
		TrailRenderer trail = null;
		if (FreeTrails.Count > 0)
		{
			trail = FreeTrails.Dequeue();
		}
		else
		{
			trail = Instantiate(TrailRef, TrailsHolder);
		}

		trail.transform.position = startPos;
		trail.gameObject.SetActive(true);

		return trail;
	}

	/// <summary>
	/// Set trail as free and wait life time.
	/// </summary>
	public void SetFreeTrail(TrailRenderer trail)
	{
		StartCoroutine(WaitVisibleTrail(trail));
	}

	/// <summary>
	/// The trail is considered busy until it disappeared.
	/// </summary>
	private IEnumerator WaitVisibleTrail(TrailRenderer trail)
	{
		trail.transform.SetParent(TrailsHolder);
		yield return new WaitForSeconds(trail.time);
		trail.Clear();
		trail.gameObject.SetActive(false);
		FreeTrails.Enqueue(trail);
	}

	protected override void AwakeSingleton()
    {
		//TrailRef.gameObject.SetActive(false);
	}
}
