using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public float offsetY = 6f;
    public float offsetZ = -2;

    Vector3 cameraPosition;

    private void LateUpdate()
    {
        cameraPosition.x = playerTransform.position.x;
        cameraPosition.y = playerTransform.position.y + offsetY;
        cameraPosition.z = playerTransform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
}
