using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarDispose : MonoBehaviour
{
    public LayerMask layer;
    public float timer;
    public Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //timer += Time.deltaTime;
        //if (timer < 5f) return;
        //if (transform.position.y < -100f) Destroy(gameObject);
        //RaycastHit hit;
        //if(!Physics.Raycast(transform.position, Vector3.down, out hit, 1000f, layer))
        //{
        //    Destroy(gameObject);
        //}
        rb.AddForce(0f, -1f * rb.mass * 19.8f, 0f);
        if (transform.position.y < -50f) Destroy(gameObject);
    }
}
