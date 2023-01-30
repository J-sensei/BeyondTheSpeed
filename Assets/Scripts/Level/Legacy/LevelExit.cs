using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public float TimeToDestroy = 1f; // After 1 second the exited chunk will delete

    public delegate void OnExitAction();
    public static event OnExitAction OnExit;

    private bool exited = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!exited)
            {
                exited = true;
                OnExit();
                StartCoroutine(WaitAndDestroy(TimeToDestroy));
            }
        }
    }

    IEnumerator WaitAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.root.gameObject);
    }
}
