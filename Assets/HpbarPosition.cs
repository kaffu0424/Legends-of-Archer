using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpbarPosition : MonoBehaviour
{
    Vector3 offsetRotation;
    Vector3 offsetPosition;

    void Start()
    {
        offsetPosition = transform.position;
        offsetRotation = transform.rotation.eulerAngles;

        Debug.Log(offsetRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + offsetPosition;
        transform.rotation = Quaternion.Euler(offsetRotation);
    }
}
