using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    // ���� �ӵ�
    public float atkSpeed;

    // ü��
    public float maxHP;
    public float currentHP;

    // ����ġ
    public float maxEXP;
    public float currentEXP;

    public PlayerStat()
    {
        maxHP = 1000f;
        currentHP = maxHP;
        atkSpeed = 1.0f;

        maxEXP = 1000f;
        currentHP = 0;
    }

    public void MultiplyAtkSpeed(float value)
    {
        atkSpeed *= value;


        // ���� ����
        if(atkSpeed > 10000f)
            atkSpeed = 10000;
    }

    public void GetDamage(float damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            currentHP = 0;

            Debug.Log("���");
        }
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
