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

    float currentDist = 0;      // ���� �Ÿ�
    float closeDist = 100f;     // ����� �Ÿ�
    float TargetDist = 100f;    // Ÿ�� �Ÿ�

    int closeDistIndex = 0;     // ���� ����� index
    int targetIndex = -1;       // Ÿ���� ������Ʈ index

    public LayerMask layerMask; // Ž�� ���̾�

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
        // #TODO:���� ��� �����
    }
}
