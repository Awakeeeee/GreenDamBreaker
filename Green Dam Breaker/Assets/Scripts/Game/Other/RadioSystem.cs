using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//My hierarchy: clearArea - player - player inner voice - radio system - helicopter - radio system call back
[RequireComponent(typeof(AudioSource))]
public class RadioSystem : MonoBehaviour 
{
	public AudioClip sosClip;
	public AudioClip replyClip;

	public float delay = 1.0f;

	AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();
	}
	 
	void OnReceivePlayerCall()
	{
		source.clip = sosClip;
		source.Play();
		Invoke("InternalReceivePlayerCall", sosClip.length + delay);
	}

	void InternalReceivePlayerCall()
	{
		//Transfer player's message to let helicopter know 
		BroadcastMessage("OnDispatchCopter");
	}

	void OnCopterReply()
	{
		source.clip = replyClip;
		source.Play();
	}
}
