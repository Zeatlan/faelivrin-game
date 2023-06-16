using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private Vector2 _minLimit;
    [SerializeField]
    private Vector2 _maxLimit;

    private float _moveSpeed = 5f;
    private float _scrollSpeed = 5f;
    private float _scrollZoneSize = 10f;

    void Update()
    {
        MoveCameraWithKeyboard();

        MoveCameraWithCursor();

        LimitMovements();
    }
    private void MoveCameraWithKeyboard()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f).normalized;
        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
    }

    private void MoveCameraWithCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (mousePosition.x < _scrollZoneSize)
        {
            transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
        }
        else if (mousePosition.x > screenWidth - _scrollZoneSize)
        {
            transform.position += Vector3.right * _scrollSpeed * Time.deltaTime;
        }

        if (mousePosition.y < _scrollZoneSize)
        {
            transform.position += Vector3.down * _scrollSpeed * Time.deltaTime;
        }
        else if (mousePosition.y > screenHeight - _scrollZoneSize)
        {
            transform.position += Vector3.up * _scrollSpeed * Time.deltaTime;
        }
    }

    private void LimitMovements()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _minLimit.x, _maxLimit.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, _minLimit.y, _maxLimit.y);
        transform.position = clampedPosition;
    }
}
