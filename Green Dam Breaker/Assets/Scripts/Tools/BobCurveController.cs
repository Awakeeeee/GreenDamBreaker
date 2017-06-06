using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A tool class that simulate character head movement while walking(nod and a bit left right movement). Which essentially is using regular curve to change camera position overtime.
/// </summary>
[System.Serializable]
public class BobCurveController
{
	//a sin curve used in head bob, value is proportion of the max bob
	public AnimationCurve headCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f), new Keyframe(1.5f, -1f), new Keyframe(2f, 0f));

	private float maxTime;

	public float maxBobOnX = 0.33f;
	public float maxBobOnY = 0.33f;

	//current curve postion. Both bob in x and y direction uses the same curv
	private float evaluatedTimeOnX;
	private float evaluatedTimeOnY;
	//character step length, this is one of the factor that determines how fast the curve is cycled
	private float characterStepLength;

	//this is kind of the change rate multiplier
	public float horizontalVerticalRatio = 2f;

	private Vector3 oringinalCamPos;

	public BobCurveController()
	{}

	public void Setup(Camera camera, float baseStep)
	{
		oringinalCamPos = camera.transform.localPosition;
		characterStepLength = baseStep;

		maxTime = headCurve.keys[headCurve.keys.Length - 1].time;
	}
		
	/// Generate a new position based on the curve
	public Vector3 Bob(float speed)
	{
		float posX = oringinalCamPos.x + headCurve.Evaluate(evaluatedTimeOnX) * maxBobOnX;
		float posY = oringinalCamPos.y + headCurve.Evaluate(evaluatedTimeOnY) * maxBobOnY;

		//Core point : when character has traveled a distance of ONE step, evaluetedTime will be 1. And head bob complete 0~1 segment on bob curve(in this case, curve time max is 1, is the whole curve cycle)
		evaluatedTimeOnX += (speed * Time.deltaTime) / characterStepLength;
		//Core point : same as above, but if ratio is 2, within ONE step, head bob complete 0~2 segment on bob curve(in this case, TWO cycles of the whole curve)
		//because the evaluate step is doubled
		evaluatedTimeOnY += (speed * Time.deltaTime) / characterStepLength * horizontalVerticalRatio;

		//loop the curve
		if(evaluatedTimeOnX > maxTime)
		{
			evaluatedTimeOnX = evaluatedTimeOnX - maxTime;
		}
		if(evaluatedTimeOnY > maxTime)
		{
			evaluatedTimeOnY = evaluatedTimeOnY - maxTime;
		}

		return new Vector3(posX, posY, 0f);
	}
}
