using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Vector2 minLimit;
    [SerializeField]
    private Vector2 maxLimit;

    private float moveSpeed = 5f;
    private float scrollSpeed = 5f;
    private float scrollZoneSize = 10f;

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
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void MoveCameraWithCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (mousePosition.x < scrollZoneSize)
        {
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
        else if (mousePosition.x > screenWidth - scrollZoneSize)
        {
            transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        }

        if (mousePosition.y < scrollZoneSize)
        {
            transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }
        else if (mousePosition.y > screenHeight - scrollZoneSize)
        {
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }
    }

    private void LimitMovements()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minLimit.x, maxLimit.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minLimit.y, maxLimit.y);
        transform.position = clampedPosition;
    }
}
