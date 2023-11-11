using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    private LineRenderer trail;
    [SerializeField] private int numOfPoints;
    [SerializeField] Vector2 pointSpacing;
    [SerializeField] private float pointsSpeed;

    private void Start()
    {
        trail = transform.GetChild(0).GetComponent<LineRenderer>();
        trail.positionCount = numOfPoints;
    }

    private void Update()
    {
        trail.SetPosition(0, transform.position);
        
        int lastIndex = trail.positionCount - 1;
        trail.SetPosition(lastIndex, Vector2.Lerp(trail.GetPosition(lastIndex), transform.position, pointsSpeed));
    }
}
