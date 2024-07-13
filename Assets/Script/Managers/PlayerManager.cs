using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    // 공격 속도
    public float atkSpeed;

    // 데미지
    public float damage;

    // 체력
    public float maxHP;
    public float currentHP;

    // 경험치
    public float maxEXP;
    public float currentEXP;

    public PlayerStat()
    {
        atkSpeed = 1.0f;

        damage = 30f;

        maxHP = 1000f;
        currentHP = 1000f;

        maxEXP = 1000f;
        currentEXP = 0;
    }

    public void MultiplyAtkSpeed(float value)
    {
        atkSpeed *= value;


        // 공속 제한
        if(atkSpeed > 10000f)
            atkSpeed = 10000;
    }

    public void GetDamage(float damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            currentHP = 0;

            // 사망 처리 추가하기.
            Debug.Log("사망");
        }

        PlayerManager.Instance.PlayerMovement.PlayerAnimator.SetTrigger("DAMAGE");
    }

    public void GetHPBoost()
    {
        maxHP += 150;
        currentHP += 150;
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

        playerMovement.InitializePlayerMovement();      // 플레이어 움직임 스크립트 초기화
        joyStickMovement.InitializeJoystick();          // 플레이어 조이스틱 스크립트 초기화
        playerTargeting.InitializePlayerTargeting();    // 플레이어 타겟팅/공격 스크립트 초기화        
    }

    public void GetHPBoost()
    {
        playerStat.GetHPBoost();        // 최대체력 / 현재 체력 추가
        playerHPbar.UpdateMaxHP();      // 추가된 체력에 맞춰 체력바 업데이트
    }
}
