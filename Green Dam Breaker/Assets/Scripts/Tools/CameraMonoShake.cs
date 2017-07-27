using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMonoShake : MonoBehaviour
{
    public float shakeAmount = 2.0f;
    public float onewayTime = 0.05f;

    private Transform shakeObj;
	private Vector3 originalPos;
    private bool goShake;
    private bool isShaking;

    void Start()
    {
		//this should be Main Camera
		shakeObj = this.transform;
		originalPos = this.transform.localPosition;

        goShake = false;
        isShaking = false;
    }

    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            TriggerShake();
//        }

        if (!goShake)
            return;

        if(!isShaking)
            StartCoroutine(MonoShake());
    }

    IEnumerator MonoShake()
    {
        isShaking = true;

		Vector3 shakeDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, -1f), Random.Range(-1f, 1f)).normalized;
		Vector3 destination = originalPos + shakeDir * shakeAmount;
        float timer = 0.0f;

        while (timer < onewayTime)
        {
            shakeObj.localPosition = Vector3.Lerp(originalPos, destination, timer / onewayTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;

        while (timer < onewayTime)
        {
            shakeObj.localPosition = Vector3.Lerp(destination, originalPos, timer / onewayTime);
            timer += Time.deltaTime;
            yield return null;
        }

        shakeObj.localPosition = originalPos;

        goShake = false;
        isShaking = false;
    }

	//External connect point
    public void TriggerShake()
    {
        goShake = true;
    }
}
