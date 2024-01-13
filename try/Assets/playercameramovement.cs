using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercameramovement : MonoBehaviour
{

    public Transform target;                            // Reference to the target (player's racing car) transform
    public Vector3 offset = new Vector3(0f, 1f, 4f);   // Offset between camera and target
    public float smoothSpeed = 0.125f;                  // Speed at which the camera follows the target
    public float rotationSpeed = 5f;                     // Speed at which the camera rotates around the target

    private Vector3 desiredPosition;                     // Desired position for the camera

    void LateUpdate()
    {
        // Calculate the desired position for the camera
        desiredPosition = target.TransformPoint(offset);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Look at the target's position
        transform.LookAt(target.position);
        /*
        // Rotate the camera around the target based on input (car steering)
        float rotationAngle = target.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        */
    }
}











