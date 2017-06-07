using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour
	where T : Component
{
	private static T instance;

	public static T Instance
	{
		get {
			if (!instance)
			{
				instance = FindObjectOfType<T>();
				if (!instance)
				{
					Debug.LogError("Create singleton error. No such object of in Scene.");
				}
			}
			return instance;
		}
	}
}
