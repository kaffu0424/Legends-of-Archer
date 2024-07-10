using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    private float currentDist = 0;     // 현재 거리
    private float targetDist = 100f;   // 타겟 거리
    private int targetIndex = -1;      // 타겟 index
    private float closeDist = 100f;    // 가장 가까운 거리
    private int closeDistIndex = 0;    // 가장 가까운 index

    [SerializeField] private bool getATarget = false;   // 타겟팅중인 몬스터가 있는지에 대한 여부
    [SerializeField] private Room currentRoomData;

    [SerializeField] private LayerMask layerMask; // 장애물 레이어
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform attackPoint;

    private PlayerMovement playerMovement;
    private JoyStickMovement playerJoystick;

    // GET / SET
    public Room CurrentRoomData { get => currentRoomData; set => currentRoomData = value; }

    public void InitializePlayerTargeting()
    {
        playerMovement = PlayerManager.Instance.PlayerMovement;
        playerJoystick = PlayerManager.Instance.JoyStickMovement;

        getATarget = false;
    }

    private void Update()
    {
        Targeting();
        AttackTarget();
    }

    public void Targeting()
    {
        if (currentRoomData == null)
            return;

        // 몬스터의 수가 0이 아닐때 ( 몬스터가 맵에 존재할때
        if (CurrentRoomData.monsterListInROOM.Count != 0)
        {
            // 초기화
            currentDist = 0f;       // 현재 거리
            closeDistIndex = 0;      // 타겟 거리
            targetIndex = -1;       // 타겟 index

            for (int i = 0; i < CurrentRoomData.monsterListInROOM.Count; i++)
            {
                // i번쨰 몬스터와 나의 거리
                currentDist = Vector3.Distance(transform.position, CurrentRoomData.monsterListInROOM[i].transform.position);

                // 플레이어 - 몬스터 레이캐스트중 충돌 -> 장애물 충돌
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, CurrentRoomData.monsterListInROOM[i].transform.position - transform.position,
                    out hit, 20f, layerMask);

                if(isHit && hit.transform.CompareTag("Enemy"))
                {
                    // 막히지않은 몬스터중 가까운 거리의 오브젝트를 타겟으로
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }    
                }

                // 장애물과 상관없이 가장 가까운 몬스터 index
                if(closeDist >= currentDist)
                {
                    closeDistIndex = i;
                    closeDist = currentDist;
                }
            }

            if(targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }

            closeDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }
        else
        {
            getATarget = false;
        }
    }

    public void AttackTarget()
    {
        // 타겟이 존재하고, 움직이고있는 상태가 아닐때
        if (getATarget && !playerJoystick.IsMoveing)
        {
            float rotationX = CurrentRoomData.monsterListInROOM[targetIndex].transform.position.x;
            float rotationY = transform.position.y;
            float rotationZ = CurrentRoomData.monsterListInROOM[targetIndex].transform.position.z;

            transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));     // 몬스터 방향 바라보기

            playerMovement.ChangeAnimationState(PlayerAnimatorState.ATTACK);    // 공격 애니메이션 실행
        }
    }

    public void Attack()
    {
        playerMovement.PlayerAnimator.SetFloat("AtkSpeed", PlayerManager.Instance.PlayerStat.atkSpeed);
        Instantiate(playerBullet, attackPoint.position, transform.rotation);
    }
}
