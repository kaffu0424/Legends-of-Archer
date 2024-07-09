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
        playerMovement.InitializePlayerMovement();      // �÷��̾� ������ ��ũ��Ʈ �ʱ�ȭ
        joyStickMovement.InitializeJoystick();          // �÷��̾� ���̽�ƽ ��ũ��Ʈ �ʱ�ȭ
        playerTargeting.InitializePlayerTargeting();    // �÷��̾� Ÿ����/���� ��ũ��Ʈ �ʱ�ȭ        
    }
}
