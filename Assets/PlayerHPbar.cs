using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPbar : MonoBehaviour
{
    Vector3 offset;
    Transform playerTransform;
    private void Start()
    {
        offset = transform.position;
        playerTransform = PlayerManager.Instance.playerTransform;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset; 
    }
}
