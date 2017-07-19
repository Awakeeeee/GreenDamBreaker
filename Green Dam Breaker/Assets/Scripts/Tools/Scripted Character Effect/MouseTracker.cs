using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A tool class, given a target camera and character, create a rotation to let the facing follows mouse move.
/// </summary>
[System.Serializable]
public class MouseTracker
{
	public float sensitiveX = 3f;
	public float sensitiveY = 3f;

	//As explicitly specified by the name, this is max\min rotation around X axis, which should be rotating Up and Down
	public float maxRotationAroundTorqueX = 90f;
	public float minRotationAroundTorqueX = -90f;

	public bool smooth;
	public float smoothSpeed;

	public bool inveresMouse = false;

	private Quaternion m_characterRotation;
	private Quaternion m_cameraRotation;

	public MouseTracker()
	{}

	public void Setup(Transform mc, Transform cam)
	{
		m_characterRotation = mc.localRotation;
		m_cameraRotation = cam.localRotation;
		GUIManager.Instance.HideCursor();
	}

	//Call in Monobehaviour update to work
	public void Track(Transform character, Transform camera)
	{
		float rotateAmountAroundY = Input.GetAxis("Mouse X") * sensitiveX;
		float rotateAmountAroundX = Input.GetAxis("Mouse Y") * sensitiveY;
		 
		//Note here left right rotation(around Y axis) is applied on Character, up down rotation(around X axis) is applied on Camera.
		//In this way, euler rotation on character obj is alway like (0, y, 0), and (x, 0, 0) on camera
		//and so I can apply my law of Quaternion(x1, 0, 0) * Quaternion(x2, 0, 0) = Quaternion(x1 + x2, 0, 0) here
		if(inveresMouse)
		{
			m_characterRotation *= Quaternion.Euler(0f, -rotateAmountAroundY, 0f);
			m_cameraRotation *= Quaternion.Euler(rotateAmountAroundX, 0f, 0f);
		}else{
			m_characterRotation *= Quaternion.Euler(0f, rotateAmountAroundY, 0f);
			m_cameraRotation *= Quaternion.Euler(-rotateAmountAroundX, 0f, 0f);
		}

		camera.localRotation = ClampRotationAroundX(m_cameraRotation);	//clamp target rotation around X

		//apply the change
		if(smooth)
		{
			character.localRotation = Quaternion.Slerp(character.transform.localRotation, m_characterRotation, smoothSpeed * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp(camera.transform.localRotation, m_cameraRotation, smoothSpeed * Time.deltaTime);
		}else
		{
			character.localRotation = m_characterRotation;
			camera.localRotation = m_cameraRotation;
		}

		if(!EventSystem.current.IsPointerOverGameObject())
		{
			CursorBehaviour();
		}
	}

	void CursorBehaviour()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			GUIManager.Instance.ShowCursor();
		}

		if(Input.GetMouseButtonDown(0))
		{
			GUIManager.Instance.HideCursor();
		}
	}

	/// Clamps the roation around X axis, limit the euler angel between min and max.
	/// specially this is done by Quaternion operations.
	//TODO I Dont fully understand this!!!
	Quaternion ClampRotationAroundX(Quaternion q)
	{
		//base knowledge: quaternion xyz components always calculated by sin(angle / 2), and w component cos(angle / 2)

		//TODO q.x/q.w gets tan, but what are the rest needed?
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;

		//now x component should be tan(angle / 2)?
		//the angleX is the euler angle number on X (already tested)
		float angleX = Mathf.Atan(q.x) * 2f * Mathf.Rad2Deg;

		angleX = Mathf.Clamp(angleX, minRotationAroundTorqueX, maxRotationAroundTorqueX);

		//back to quaternion
		//Alternatively: 
		//q = Quaternion.Euler(angleX, 0f, 0f);
		q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

		return q;
	}
}
