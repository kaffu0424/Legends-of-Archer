using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargeting instance;

    public bool getATarget = false;

    float currentDist = 0;      // 현재 거리
    float closeDist = 100f;     // 가까운 거리
    float TargetDist = 100f;    // 타겟 거리

    int closeDistIndex = 0;     // 가장 가까운 index
    int targetIndex = -1;       // 타겟팅 오브젝트 index

    public LayerMask layerMask; // 탐색 레이어

    public List<GameObject> MonsterListInROOM = new List<GameObject>();

    public GameObject playerBullet;
    public Transform AttackPoint;

    void Update()
    {
        if(MonsterListInROOM.Count != 0)
        {
            currentDist = 0f;
            closeDistIndex = 0;
            targetIndex = -1;

            for (int i = 0; i < MonsterListInROOM.Count; i++)
            {
                currentDist = Vector3.Distance(transform.position, MonsterListInROOM[i].transform.position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterListInROOM[i].transform.position - transform.position,
                    out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Enemy"))
                {
                     if(TargetDist >= currentDist)
                    {
                        targetIndex = i;
                        TargetDist = currentDist;
                    }
                }
                
                if(closeDist >= currentDist)
                {
                    closeDistIndex = i;
                    closeDist = currentDist;
                }
            }

            if (targetIndex == -1)
                targetIndex = closeDistIndex;

            closeDist = 100f;
            TargetDist = 100f;
            getATarget = true;
        }

        if(getATarget && !JoyStickMovement.Instance.isMoveing)
        {
            transform.LookAt(new Vector3( MonsterListInROOM[targetIndex].transform.position.x, transform.position.y, MonsterListInROOM[targetIndex].transform.position.z));
            // Attack();

            if (PlayerMovement.Instance.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                PlayerMovement.Instance.ChangeState(PlayerAnimatorState.ATTACK);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (MonsterListInROOM.Count != 0)
        {
            for (int i = 0; i < MonsterListInROOM.Count; i++)
            {
                Gizmos.DrawLine(transform.position, MonsterListInROOM[i].transform.position);
            }
        }
    }

    public void Attack()
    {
        // #TODO:공격 기능 만들기
    }
}
