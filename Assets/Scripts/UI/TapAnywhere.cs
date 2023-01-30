using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapAnywhere : Singleton<TapAnywhere>
{
    public bool Tap { get; private set; }

    protected override void AwakeSingleton()
    {
        
    }

    void Update()
    {
        if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) &&
            EventSystem.current.IsPointerOverGameObject())
        {
            Tap = true;
            Debug.Log("Yes");
        }
        else
        {
            Tap = false;
        }
    }
}
