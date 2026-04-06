using UnityEngine;

/// <summary>
/// 设置单层纯色深空背景。
/// </summary>
[RequireComponent(typeof(Camera))]
public class SpaceBackgroundController : MonoBehaviour
{
    [Header("背景颜色")]
    public Color backgroundColor = new Color(0.03f, 0.07f, 0.18f, 1f);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null) return;

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = backgroundColor;
    }
}
