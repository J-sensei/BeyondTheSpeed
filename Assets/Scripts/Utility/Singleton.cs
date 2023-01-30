using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton template to enable singleton of a class
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [Tooltip("Do not destroy the target object when loading a new scene")]
    [SerializeField] private bool dontDestroyOnLoad;

    private static T instance;
    public static T Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            AwakeSingleton();
        }
        else
        {
            // Destroy the object if the instance is already available
            Destroy(gameObject.GetComponent<T>());
        }
    }

    /// <summary>
    /// Events to trigger when create an instance
    /// </summary>
    protected abstract void AwakeSingleton();
}
