using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InnerVoice : MonoBehaviour 
{
	public GameObject landingArea;

	public AudioClip startClip;
	public AudioClip callClip;

	public float delay = 1.5f;

	AudioSource source;

	public bool hasCalled = false; //temp

	void Start()
	{
		source = GetComponent<AudioSource>();
		StartGuideVoice();
	}

	void StartGuideVoice()
	{
		source.clip = startClip;
		source.Play();
	}
		
	//Message method, when clear area is confirmed
	void OnCallHelicopter()
	{
		if(hasCalled)
			return;

		source.clip = callClip;
		source.Play();

		//TODO disable player movement

		//Real world: radio wave goes to radio system, then after a period of time, radio wave is transfered to the receiver
		//Game simulation: send message to call method in radio system 
		Invoke("InternalSend", callClip.length + delay);
	}

	void InternalSend()
	{
		SendMessageUpwards("OnReceivePlayerCall");
		hasCalled = true;

		Instantiate(landingArea, this.transform.position, Quaternion.identity);

		//TODO enable player movement
	}
}
