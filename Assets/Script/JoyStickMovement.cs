using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    public static JoyStickMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<JoyStickMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("joystickMovement");
                    instance = instanceContainer.AddComponent<JoyStickMovement>();
                }
            }
            return instance;
        }
    }
    private static JoyStickMovement instance;


    public GameObject smallStick;
    public GameObject bgStick;

    Vector3 stickFirstPosition;
    Vector3 joyStickDefalutPosition;
    public Vector3 joyVec;
    float stickRadius;

    public bool isMoveing;
    private void Start()
    {
        stickRadius = bgStick.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickDefalutPosition = bgStick.transform.position;
    }

    public void PointDown()
    {
        bgStick.SetActive(true);                                
        bgStick.transform.position = Input.mousePosition;       
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        isMoveing = true;

        PlayerMovement.Instance.WalkPlayer();
    }   

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEvemtData = baseEventData as PointerEventData;

        Vector3 dragPosition = pointerEvemtData.position;
        joyVec = (dragPosition - stickFirstPosition).normalized;

        float stickDistance = Vector3.Distance(dragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        else
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bgStick.transform.position = joyStickDefalutPosition;
        smallStick.transform.position = joyStickDefalutPosition;

        isMoveing = false;

        PlayerMovement.Instance.IdlePlayer();
    }
}
