using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    [SerializeField] private bool getATarget = false;   // Ÿ�������� ���Ͱ� �ִ����� ���� ����

    private float currentDist = 0;      // ���� �Ÿ�
    private float targetDist = 100f;    // Ÿ�� �Ÿ�
    private int targetIndex = -1;       // Ÿ�� index

    [SerializeField] private LayerMask targetLayerMask; // Ž�� ���̾�

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
        // ������ ���� 0�� �ƴҶ� ( ���Ͱ� �ʿ� �����Ҷ�
        if (monsterListInROOM.Count != 0)
        {
            // �ʱ�ȭ
            currentDist = 0f;       // ���� �Ÿ�
            targetDist = 100f;      // Ÿ�� �Ÿ�
            targetIndex = -1;       // Ÿ�� index

            for (int i = 0; i < monsterListInROOM.Count; i++)
            {
                // i���� ���Ϳ� ���� �Ÿ�
                currentDist = Vector3.Distance(transform.position, monsterListInROOM[i].transform.position);

                // �÷��̾� - ���� ����ĳ��Ʈ�� �浹 -> ��ֹ� �浹
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, monsterListInROOM[i].transform.position - transform.position,
                    out hit, 20f, targetLayerMask);

                // ��ֹ��� �浹������ Ÿ���� �ĺ��� ��������.
                if (isHit)
                    continue;

                // ����� �Ÿ� �������� Ÿ���� ������Ʈ
                if(hit.transform.CompareTag("Enemy"))
                {
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }
                }
            }

            // Ÿ�������� ���Ͱ� ������ true , ������ false
            getATarget = targetIndex != -1;

            // Ÿ���� �����ϰ�, �����̰��ִ� ���°� �ƴҶ�
            if(getATarget && !playerJoystick.IsMoveing)
            {
                float rotationX = monsterListInROOM[targetIndex].transform.position.x;
                float rotationY = transform.position.y;
                float rotationZ = monsterListInROOM[targetIndex].transform.position.z;

                transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));         // ���� ���� �ٶ󺸱�
                Attack();                                                               // ���� ����

                playerMovement.ChangeAnimationState(PlayerAnimatorState.ATTACK);                 // ���� �ִϸ��̼� ����
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
