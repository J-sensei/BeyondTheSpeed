using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;

    private void Update()
    {
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        //mask.fillAmount = fillAmount;
        mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, 10f * Time.deltaTime);
        fill.color = color;
    }
}
