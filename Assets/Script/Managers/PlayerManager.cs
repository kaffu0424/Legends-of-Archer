using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public float atkSpeed;

    public PlayerStat()
    {
        atkSpeed = 1.0f;
    }

    /// <summary> 
    /// value 1.0 -> 1�� ( ���� X )  value 2.0 -> 2��
    /// ���� ���ݼӵ� �������� ������
    /// </summary>
    public void MultiplyAtkSpeed(float value)
    {
        atkSpeed *= value;


        // ���� ����
        if(atkSpeed > 10000f)
            atkSpeed = 10000;
    }
}

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

    [SerializeField] private PlayerStat playerStat;
    // GET/SET
    public JoyStickMovement JoyStickMovement => joyStickMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerTargeting PlayerTargeting => playerTargeting;
    public CameraMovement CameraMovement => cameraMovement;
    public PlayerStat PlayerStat => playerStat;

    protected override void InitManager()
    {
        playerStat = new PlayerStat();

        playerMovement.InitializePlayerMovement();      // �÷��̾� ������ ��ũ��Ʈ �ʱ�ȭ
        joyStickMovement.InitializeJoystick();          // �÷��̾� ���̽�ƽ ��ũ��Ʈ �ʱ�ȭ
        playerTargeting.InitializePlayerTargeting();    // �÷��̾� Ÿ����/���� ��ũ��Ʈ �ʱ�ȭ        
    }
}
