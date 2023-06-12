//source: https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class CanvasHelper : MonoBehaviour
{
    private static List<CanvasHelper> helpers = new List<CanvasHelper>();

    private static bool screenChangeVarsInitialized = false;
    private static ScreenOrientation lastOrientation = ScreenOrientation.LandscapeLeft;
    private static Rect lastSafeArea = Rect.zero;

    private Canvas canvas;
    private RectTransform rectTransform;
    private RectTransform safeAreaTransform;

    void Awake()
    {
        if (!helpers.Contains(this))
            helpers.Add(this);

        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        safeAreaTransform = transform.Find("SafeArea") as RectTransform;

        if (!screenChangeVarsInitialized)
        {
            lastOrientation = Screen.orientation;
            lastSafeArea = Screen.safeArea;

            screenChangeVarsInitialized = true;
        }

        ApplySafeArea();
    }

    void Update()
    {
        if (helpers[0] != this)
            return;

        if (Application.isMobilePlatform && Screen.orientation != lastOrientation)
            OrientationChanged();
    }

    void ApplySafeArea()
    {
        if (safeAreaTransform == null)
            return;

        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }

    void OnDestroy()
    {
        if (helpers != null && helpers.Contains(this))
            helpers.Remove(this);
    }

    private static void OrientationChanged()
    {
        //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);
        lastOrientation = Screen.orientation;
    }
}