using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSound : MonoBehaviour 
{

	public static PlayerSound Instance;

	public AudioClip SoundToPlay;
	public float VolumeToPlay;

	private AudioSource PlayerAudio;

	public AudioClip Footsteps1;
	public AudioClip Footsteps2;

	public float minFootstepPitch = 0.9f;
	public float maxFootstepPitch = 1.1f;
	public float minFootstepVolume = 0.4f;
	public float maxFootstepVolume = 0.6f;

	void Awake()
	{
		if (Instance != this)
			Instance = this;

		if (PlayerAudio == null)
			PlayerAudio = GetComponentInChildren<AudioSource>();
	}

	public void PlayCustomSound()
	{
		PlayerAudio.pitch = 1f;
		PlayerAudio.PlayOneShot(SoundToPlay, VolumeToPlay);
	}

	public void PlayFootStep()
	{
		int random = Random.Range(0, 1);

		float pitch = Random.Range(minFootstepPitch, maxFootstepPitch);

		PlayerAudio.pitch = pitch;

		if (random == 0)
		{
			PlayerAudio.PlayOneShot(Footsteps1, Random.Range(minFootstepVolume, maxFootstepVolume));
		}
		else
			PlayerAudio.PlayOneShot(Footsteps2, Random.Range(minFootstepVolume, maxFootstepVolume));

	}
}
