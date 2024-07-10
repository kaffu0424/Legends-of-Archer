using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpbarPosition : MonoBehaviour
{
    Vector3 offsetRotation;

    void Start()
    {
        // �ʱ�ȭ
        offsetRotation = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        // Rotation�� ������Ű��
        transform.rotation = Quaternion.Euler(offsetRotation);
    }
}
