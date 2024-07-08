using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    // JoyStick
    [SerializeField] private GameObject smallStick;
    [SerializeField] private GameObject bgStick;

    private Vector3 stickFirstPosition;                
    private Vector3 joyStickDefalutPosition;
    private float stickRadius;

    // JoyStick Data
    private Vector3 joyVec;
    private bool isMoveing;

    // playerMovement
    private PlayerMovement playerMovement;

    // GET / SET
    public bool IsMoveing => isMoveing;
    public Vector3 JoyVec => joyVec;

    public void InitializeJoystick()
    {
        stickRadius = bgStick.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickDefalutPosition = bgStick.transform.position;

        playerMovement = PlayerManager.Instance.PlayerMovement;
    }

    public void PointDown()
    {
        bgStick.SetActive(true);                                
        bgStick.transform.position = Input.mousePosition;       
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;

        isMoveing = true;

        playerMovement.WalkPlayerAnimation();
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

        playerMovement.IdlePlayerAnimation();
    }
}
