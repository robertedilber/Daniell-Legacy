using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // Instanciated version of this MonoBehaviour
    public static T Instance { get; set; }

    protected virtual void Awake()
    {
        // If the Instance is already set, destroy this instance
        if (Instance != null)
            Destroy(this);
        else
            Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        // Set the instance to null when this instance is destroyed
        Instance = null;
    }
}
