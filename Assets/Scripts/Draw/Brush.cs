using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Brush : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;

    private List<Vector2> points;
    private float minDistance = .1f;

    public void UpdateLine(Vector2 pos) {
        if (points == null) {
            points = new List<Vector2>();
            SetPoint(pos);
            return;
        }

        if (Vector2.Distance(points.Last(), pos) > minDistance) {
            SetPoint(pos);
        }
    }

    private void SetPoint(Vector2 point) {
        points.Add(point);

        lineRend.positionCount = points.Count;
        lineRend.SetPosition(points.Count - 1, point);
    }
}
