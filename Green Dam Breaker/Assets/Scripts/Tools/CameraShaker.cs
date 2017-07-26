using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [Header("Random Shake Value Monitor")]
    public float randomAmount;
    public float randomtime;
    public float randomDecayFactor = 1.0f;

    private Transform shakeObj;
    private Vector3 originalPos;
    private bool goShake;

    void Start()
    {
		//most time this should be the camera
		shakeObj = this.transform;
		originalPos = this.transform.position;
    }

    void OnEnable()
    {
        goShake = false;
    }

    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            TriggerRandomShake(0.5f, 0.05f, 1.0f);
//        }

        if (!goShake)
            return;

        if (randomtime > 0f)
        {
            Vector3 shakeAdd = Random.insideUnitSphere * randomAmount;
            shakeObj.localPosition = originalPos + shakeAdd;
            randomtime -= Time.deltaTime * randomDecayFactor;
        }
        else
        {
            shakeObj.localPosition = originalPos;
            randomtime = 0f;
            goShake = false;
        }

    }

	public void TriggerRandomShake(float _amount = 0.5f, float _time = 0.05f, float _decay = 1.0f)
    {
        goShake = true;
        randomAmount = _amount;
        randomtime = _time;
        randomDecayFactor = _decay;
    }
}
