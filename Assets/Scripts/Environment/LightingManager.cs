using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Variables")]
    [SerializeField, Range(0, 24f) ] private float DayTime;

    /// <summary>
    /// 0 - 5.5 && 18.5 - 24 turn the lighting off
    /// </summary>
    public float CurrentTime { get { return DayTime; } }

    private void Update()
    {
        if (Preset == null) return;
        if (Application.isPlaying)
        {
            DayTime += Time.deltaTime * 0.2f;
            DayTime %= 24; // Clamp betwenn 0 - 24
            UpdateLighting(DayTime / 24f);
        }
        else
        {
            UpdateLighting(DayTime / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null) return;

        if(RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
