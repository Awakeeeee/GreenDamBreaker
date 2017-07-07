using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager> 
{
	public AudioSource source2D;
	public AudioSource source3D;

	private AudioSource[] audioSources;
	private float[] defaultVolumes;

	void Start()
	{
		audioSources = FindObjectsOfType<AudioSource>();
		defaultVolumes = new float[audioSources.Length];

		for(int i = 0; i < audioSources.Length; i++)
		{
			defaultVolumes[i] = audioSources[i].volume;
		}
	}

	public void PlayClip2D(AudioClip clip)
	{
		source2D.PlayOneShot(clip);
	}

	public void SetVolume(float multipler)
	{
		for(int i = 0; i < audioSources.Length; i++)
		{
			audioSources[i].volume = defaultVolumes[i] * multipler;
		}
	}
}
