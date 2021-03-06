﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool doMovement = true;
    public bool doPan = false;
    public float panSpeed = 10f;
    public float panBorderMultiplier = 0.0025f;

    public float zoomSpeed = 1;
    private float targetOrtho;
    private Vector3 targetPosition;
    private Vector3 oppPosition;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;

    // Update is called once per frame
    void Update()
    {
        if (!doMovement)
        {
            return;
        }
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= (Screen.height * (1 - panBorderMultiplier)) && doPan))
        {
            transform.Translate(new Vector3(0, panSpeed * Time.deltaTime, 0), Space.World);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= (Screen.height * panBorderMultiplier) && doPan))
        {
            transform.Translate(new Vector3(0, -panSpeed * Time.deltaTime, 0), Space.World);
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= (Screen.width * panBorderMultiplier) && doPan))
        {
            transform.Translate(new Vector3(-panSpeed * Time.deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= (Screen.width * (1 - panBorderMultiplier)) && doPan))
        {
            transform.Translate(new Vector3(panSpeed * Time.deltaTime, 0, 0), Space.World);
        }
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
        if (Input.GetKey("space"))
        {
            transform.position = targetPosition;
            
        }
        if (Input.GetKey("k"))
        {
            transform.position = oppPosition;

        }
    }


    void Start()
    {
        targetOrtho = Camera.main.orthographicSize;
        targetPosition = Camera.main.transform.position;
        oppPosition = targetPosition;
        oppPosition.Set(oppPosition.x + 43, oppPosition.y, oppPosition.z);
    }
}
