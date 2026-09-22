using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Header("Aspect ratio you designed the game for (Game view)")]
    public float targetAspect = 16f / 9f;

    [Header("Letterbox = black bars, never crops. Off = zooms out to show more level.")]
    public bool letterbox = true;

    private Camera cam;
    private float baseOrthoSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        baseOrthoSize = cam.orthographicSize;
    }

    void Start() { ApplyFit(); }

    void ApplyFit()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scale = windowAspect / targetAspect;

        if (letterbox)
        {
            Rect rect = cam.rect;
            if (scale < 1f)      // narrower screen (portrait-ish) -> bars top/bottom
            {
                rect.width = 1f;
                rect.height = scale;
                rect.x = 0f;
                rect.y = (1f - scale) / 2f;
            }
            else                 // wider screen -> bars left/right
            {
                float scaleWidth = 1f / scale;
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0f;
            }
            cam.rect = rect;
        }
        else
        {
            // No bars: keep horizontal view, expand vertical view instead
            cam.rect = new Rect(0, 0, 1, 1);
            cam.orthographicSize = (scale < 1f) ? baseOrthoSize / scale : baseOrthoSize;
        }
    }
}