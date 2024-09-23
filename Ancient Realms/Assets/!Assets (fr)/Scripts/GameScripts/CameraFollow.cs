using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Camera cm;
    [SerializeField] public Transform target;

    // Offset distance between the camera and the target
    public float offsetX;  

    // How smoothly the camera follows the target
    public float smoothSpeed = 0.125f;
    void LateUpdate()
    {
        Vector3 currentPos = transform.position;

        // Compute the desired position only for the X-axis
        float desiredX = target.position.x + offsetX;



        // Update the camera's position with the new X and keep the original Y and Z
        transform.position = new Vector3(desiredX, currentPos.y, currentPos.z);

    }

}
