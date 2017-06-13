using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotate aroung z axis and turn back.	
//Alternatively use animation.
[System.Serializable]
public class RecoilEffect 
{
	public AnimationCurve changeCurve;	//curve range from (0,0) to (1,1), representing time/value proportion
	public float time;
	public float maxRotationZ;

	private Quaternion originalRotation;

	public RecoilEffect()
	{
		
	}

	public void SetUp(Quaternion origin)
	{
		originalRotation = origin;
	}

	public IEnumerator DoRecoil(Transform gunTarget)
	{
		float timer = 0.0f;

		while(timer < time)
		{
			float newZ = changeCurve.Evaluate(timer / time) * maxRotationZ;
			Quaternion newQ = Quaternion.Euler(0f, 0f, newZ);
			gunTarget.localRotation = newQ;
			timer += Time.deltaTime;
			yield return null;
		}

		gunTarget.localRotation = originalRotation;
	}
}
