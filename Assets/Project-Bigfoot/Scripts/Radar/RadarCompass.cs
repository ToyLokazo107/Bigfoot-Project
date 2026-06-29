using System.Collections.Generic;
using UnityEngine;

public class RadarCompassSprites : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public RectTransform radarContent;

    [Header("Settings")]
    public float pixelsPerDegree = 6f;
    public float visibleRange = 60f;

    private List<CompassMarker> markers = new List<CompassMarker>();

    private class CompassMarker
    {
        public RectTransform rect;
        public float degree;
        public float yPosition;
    }

    private void Start()
    {
        for (int i = 0; i < radarContent.childCount; i++)
        {
            RectTransform child = radarContent.GetChild(i).GetComponent<RectTransform>();

            if (child == null) continue;

            float degree = GetDegreeFromName(child.name);

            markers.Add(new CompassMarker
            {
                rect = child,
                degree = degree,
                yPosition = child.anchoredPosition.y
            });
        }
    }

    private void Update()
    {
        float cameraY = playerCamera.eulerAngles.y;

        for (int i = 0; i < markers.Count; i++)
        {
            float difference = Mathf.DeltaAngle(cameraY, markers[i].degree);

            float xPosition = difference * pixelsPerDegree;

            markers[i].rect.anchoredPosition = new Vector2(
                xPosition,
                markers[i].yPosition
            );

            markers[i].rect.gameObject.SetActive(Mathf.Abs(difference) <= visibleRange);
        }
    }

    private float GetDegreeFromName(string markerName)
    {
        switch (markerName)
        {
            case "N": return 0f;
            case "NE": return 45f;
            case "E": return 90f;
            case "SE": return 135f;
            case "S": return 180f;
            case "SO": return 225f;
            case "O": return 270f;
            case "NO": return 315f;
        }

        if (float.TryParse(markerName, out float value))
        {
            return value;
        }

        return 0f;
    }
}