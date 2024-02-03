using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the line controller draws visual lines between two nodes
/// the line controller is puerly visual, actual connections are stored in the UINode class
/// </summary>
public class LineRenderer : MonoBehaviour
{
    private UnityEngine.LineRenderer lr;
    public Transform[] points;

    private void Awake()
    {
        lr = GetComponent<UnityEngine.LineRenderer>();
        points = new Transform[2];
    }

    public void SetupLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }

    private void Update()
    {
        for(int i=0; i<points.Length; i++)
        {
            if(points[i] != null)
                lr.SetPosition(i, points[i].position);
        }
    }
}
