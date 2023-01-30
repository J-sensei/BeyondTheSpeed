using UnityEngine;

public class TouchControl : MonoBehaviour
{
    bool pressed;
    RectTransform rectTransform;
    Vector3 initialScale;

    public bool Pressed { get { return pressed; } }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }
    public void ButtonDown()
    {
        pressed = true;
        rectTransform.localScale = initialScale * 0.85f;
    }

    public void ButtonUp()
    {
        pressed = false;
        rectTransform.localScale = initialScale;
    }
}
