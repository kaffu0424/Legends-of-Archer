using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    [SerializeField] private bool getATarget = false;   // 타겟팅중인 몬스터가 있는지에 대한 여부

    private float currentDist = 0;      // 현재 거리
    private float targetDist = 100f;    // 타겟 거리
    private int targetIndex = -1;       // 타겟 index

    [SerializeField] private LayerMask targetLayerMask; // 탐색 레이어

    [SerializeField] private List<GameObject> monsterListInROOM = new List<GameObject>();

    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform attackPoint;

    private PlayerMovement playerMovement;
    private JoyStickMovement playerJoystick;

    // GET / SET
    public List<GameObject> MonsterListInROOM { get => monsterListInROOM; set => monsterListInROOM = value; }
    public void InitializePlayerTargeting()
    {
        playerMovement = PlayerManager.Instance.PlayerMovement;
        playerJoystick = PlayerManager.Instance.JoyStickMovement;

        getATarget = false;
    }

    public void Targeting()
    {
        // 몬스터의 수가 0이 아닐때 ( 몬스터가 맵에 존재할때
        if (monsterListInROOM.Count != 0)
        {
            // 초기화
            currentDist = 0f;       // 현재 거리
            targetDist = 100f;      // 타겟 거리
            targetIndex = -1;       // 타겟 index

            for (int i = 0; i < monsterListInROOM.Count; i++)
            {
                // i번쨰 몬스터와 나의 거리
                currentDist = Vector3.Distance(transform.position, monsterListInROOM[i].transform.position);

                // 플레이어 - 몬스터 레이캐스트중 충돌 -> 장애물 충돌
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterListInROOM[i].transform.position - transform.position,
                    out hit, 20f, targetLayerMask);

                // 장애물에 충돌했을때 타겟팅 후보로 넣지않음.
                if (isHit)
                    continue;

                // 가까운 거리 기준으로 타겟을 업데이트
                if(hit.transform.CompareTag("Enemy"))
                {
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }
                }
            }

            // 타겟팅중인 몬스터가 있을때 true , 없을때 false
            getATarget = targetIndex != -1;

            // 타겟이 존재하고, 움직이고있는 상태가 아닐때
            if(getATarget && !playerJoystick.IsMoveing)
            {
                float rotationX = monsterListInROOM[targetIndex].transform.position.x;
                float rotationY = transform.position.y;
                float rotationZ = monsterListInROOM[targetIndex].transform.position.z;

                transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));         // 몬스터 방향 바라보기
                Attack();                                                               // 공격 실행

                playerMovement.ChangeAnimationState(PlayerAnimatorState.ATTACK);                 // 공격 애니메이션 실행
            }

        }
    }


    public void Attack()
    {
        GameObject bullet = Instantiate(playerBullet);
        bullet.transform.rotation = transform.rotation;
        bullet.transform.position = attackPoint.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (monsterListInROOM.Count != 0)
        {
            for (int i = 0; i < monsterListInROOM.Count; i++)
            {
                Gizmos.DrawLine(transform.position, monsterListInROOM[i].transform.position);
            }
        }
    }

}
