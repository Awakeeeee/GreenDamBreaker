using UnityEngine;
using System.Collections;

/// <summary>
/// Derive this to be a persistent singleton MonoBehaviour.
/// </summary>
public class PersistentSingletonBase<T> : MonoBehaviour
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
					Debug.LogWarning("Create persistent singleton error. No " + typeof(T).ToString() + " in Scene.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
