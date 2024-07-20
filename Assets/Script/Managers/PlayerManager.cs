using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    EnemyBounce,    // ���� ƨ���      
    MultiAttack,    // �Ѿ� �ѹ� �� ���  - �Ϸ�
    AddStraight,    // ���� ȭ�� �߰�     - �Ϸ�
    AddDiagonal,    // �缱 ȭ�� �߰�     - �Ϸ�
    Bounce          // �� ƨ���
}

[System.Serializable]
public class PlayerStat
{
    public float moveSpeed;

    // ���� �ӵ�
    public float atkSpeed;

    // ������
    public float damage;

    // ü��
    public float maxHP;
    public float currentHP;

    // ����ġ
    public float maxEXP;
    public float currentEXP;
    public int level;

    public int[] skills;

    public PlayerStat()
    {
        moveSpeed = 5f;

        atkSpeed = 1.0f;

        damage = 30f;

        maxHP = 1000f;
        currentHP = 1000f;

        maxEXP = 1000f;
        currentEXP = 0;
        level = 1;

        skills = new int[Enum.GetValues(typeof(SkillName)).Length];
    }


    public void GetDamage(float damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            currentHP = 0;

            // ��� ó�� �߰��ϱ�.
            Debug.Log("���");
        }

        PlayerManager.Instance.PlayerMovement.PlayerAnimator.SetTrigger("DAMAGE");
    }

    // �ִ�ü�� ����
    public void GetHPBoost()
    {
        maxHP += 150;
        currentHP += 150;
    }

    // ���� ����
    public void MultiplyAtkSpeed(float value)
    {
        atkSpeed *= value;

        // ���� ����
        if(atkSpeed > 10000f)
            atkSpeed = 10000;
    }

    // ���ݷ� ����
    public void GetDamageBoost(float value)
    {
        damage *= value;
    }

    public void GetExp(float value)
    {
        currentEXP += value;
        if(currentEXP >= maxEXP)
        {
            // ���� ��
            level++;
            // ����ġ max����ġ ��ŭ ����
            currentEXP -= maxEXP;

            // ���Ըӽ� ����
            RouletteManager.Instance.SlotMachineSpin();
        }
    }

    public void GetMoveSpeed(float value)
    {
        moveSpeed += value;
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

    // HP bar
    [SerializeField] private PlayerHPbar playerHPbar;

    // Exp bar
    [SerializeField] private PlayerExpBar playerExpBar;

    // Player Stat
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

    public void GetHPBoost()
    {
        playerStat.GetHPBoost();        // �ִ�ü�� / ���� ü�� �߰�
        playerHPbar.UpdateMaxHP();      // �߰��� ü�¿� ���� ü�¹� ������Ʈ
    }

    public void GetExp(float value)
    {
        playerStat.GetExp(value);
        playerExpBar.UpdateExpBar(ref playerStat);
    }
}
