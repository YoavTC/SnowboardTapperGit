using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Transform rightWall, leftWall;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float leftX = mainCamera.ViewportToWorldPoint(Vector3.zero).x * 1.1f;
        float rightX = mainCamera.ViewportToWorldPoint(Vector3.one).x * 1.1f;

        rightWall.position = new Vector3(rightX, 0);
        leftWall.position = new Vector3(leftX, 0);
    }
}
