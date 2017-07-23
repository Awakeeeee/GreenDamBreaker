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
					Debug.LogError("Create persistent singleton error. No such object of in Scene.");
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
