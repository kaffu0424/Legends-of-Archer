using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpbarPosition : MonoBehaviour
{
    Vector3 offsetRotation;

    void Start()
    {
        // 초기화
        offsetRotation = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        // Rotation값 고정시키기
        transform.rotation = Quaternion.Euler(offsetRotation);
    }
}
