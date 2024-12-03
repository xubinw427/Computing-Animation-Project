using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform rodTip; // Attach the tip of the rod here.

    [SerializeField]
    private Transform hook; // Attach the hook object here.

    [SerializeField]
    private List<Transform> segments; // Add all segment transforms here.


    private void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned!");
            enabled = false;
            return;
        }

        if (rodTip == null || hook == null || segments == null || segments.Count == 0)
        {
            Debug.LogError("Ensure that rodTip, hook, and segments are properly assigned!");
            enabled = false;
            return;
        }
        // Set the initial number of positions for the LineRenderer
        lineRenderer.positionCount = segments.Count + 2; // +2 for rodTip and hook
    }

    private void LateUpdate()
    {
        UpdateFishingLine();
    }

    private void UpdateFishingLine()
    {
        // Ensure LineRenderer matches all segments and hook
        lineRenderer.SetPosition(0, rodTip.position); // Start at rod tip

        for (int i = 0; i < segments.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, segments[i].position); // Set each segment position
        }

        // End at hook position
        lineRenderer.SetPosition(segments.Count + 1, hook.position);
    }
}


