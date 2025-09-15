using System;
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    void Update()
    {
        UpdateLookAngle();
        UpdatePosition();
    }

    void UpdateLookAngle()
    {
        if (!Input.GetMouseButton(1))
            return;
        
        Vector3 lookAngle = transform.eulerAngles;
        lookAngle.y += Input.GetAxis("Mouse X") * lookSpeed;
        lookAngle.x -= Input.GetAxis("Mouse Y") * lookSpeed;
        transform.eulerAngles = lookAngle;
    }

    void UpdatePosition()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Convert.ToInt32(Input.GetKey(KeyCode.E)) - Convert.ToInt32(Input.GetKey(KeyCode.Q));

        Vector3 direction = (transform.right * x + transform.up * y + transform.forward * z).normalized;
        transform.position += direction * (moveSpeed * Time.deltaTime);
    }
}
