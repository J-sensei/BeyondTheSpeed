using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To stop when infront has slower car
public class AICarCheck : MonoBehaviour
{
    public List<GameObject> colliderList = new List<GameObject>();
    private AICarController controller;

    private void Start()
    {
        controller = GetComponentInParent<AICarController>();
    }

    private void FixedUpdate()
    {
        if(colliderList.Count > 0)
        {
            controller.ShouldDrive = false;
        }
        else
        {
            controller.ShouldDrive = true;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Tag: " + collider.gameObject.tag);
        if (!colliderList.Contains(collider.gameObject) && collider.gameObject.tag == "AICar")
        {
            colliderList.Add(collider.gameObject);
            //Debug.Log("Added " + gameObject.name);
            //Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (colliderList.Contains(collider.gameObject) && collider.gameObject.tag == "AICar")
        {
            colliderList.Remove(collider.gameObject);
            //Debug.Log("Removed " + gameObject.name);
            //Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }
}
