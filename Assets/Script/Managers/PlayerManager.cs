using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Transform playerTransform;

    // Movement
    [SerializeField] private PlayerMovement playerMovement;

    // JoyStick
    [SerializeField] private JoyStickMovement joyStickMovement;

    // Targeting
    [SerializeField] private PlayerTargeting playerTargeting;

    // Camera
    [SerializeField] private CameraMovement cameraMovement;

    // GET/SET
    public JoyStickMovement JoyStickMovement => joyStickMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerTargeting PlayerTargeting => playerTargeting;

    public CameraMovement CameraMovement => cameraMovement;
    protected override void InitManager()
    {
        playerMovement.InitializePlayerMovement();      // 플레이어 움직임 스크립트 초기화
        joyStickMovement.InitializeJoystick();          // 플레이어 조이스틱 스크립트 초기화
        playerTargeting.InitializePlayerTargeting();    // 플레이어 타겟팅/공격 스크립트 초기화        
    }
}
