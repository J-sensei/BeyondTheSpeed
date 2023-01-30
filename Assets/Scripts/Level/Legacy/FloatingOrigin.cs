using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float Threshold = 100f; // How much distance to reset the position to orogin
    public Transform Target;
    public LevelGeneratorLegacy LvlGenerator;

    private void LateUpdate()
    {
        Vector3 cameraPos = Target.position;
        cameraPos.y = 0; // Dont reset the Y

        if(cameraPos.magnitude > Threshold)
        {
            for(int i = 0; i < SceneManager.sceneCount; i++)
            {
                foreach (GameObject g in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    g.transform.position -= cameraPos;
                }
            }

            Vector3 delta = Vector3.zero - cameraPos;
            //Debug.Log("Spawn Pos Before: " + LvlGenerator.SpawnOrigin);
            LvlGenerator.UpdateSpawnOrigin(delta);

            //Debug.Log("Resetted Origin: " + delta);
            //Debug.Log("Spawn Pos After: " + LvlGenerator.SpawnOrigin);
        }
    }
}
