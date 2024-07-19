using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    delegate void AttackDelegate();
    AttackDelegate AttackDelegatechain;


    private float currentDist = 0;     // ���� �Ÿ�
    private float targetDist = 100f;   // Ÿ�� �Ÿ�
    private int targetIndex = -1;      // Ÿ�� index
    private float closeDist = 100f;    // ���� ����� �Ÿ�
    private int closeDistIndex = 0;    // ���� ����� index

    [SerializeField] private bool getATarget = false;   // Ÿ�������� ���Ͱ� �ִ����� ���� ����
    [SerializeField] private Room currentRoomData;

    [SerializeField] private LayerMask layerMask; // ��ֹ� ���̾�
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

        AttackDelegatechain += DefaultAttack;
        AttackDelegatechain += AddStraightAttack;
        AttackDelegatechain += AddDiagonalAttack;
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

        // ������ ���� 0�� �ƴҶ� ( ���Ͱ� �ʿ� �����Ҷ�
        if (CurrentRoomData.monsterListInROOM.Count != 0)
        {
            // �ʱ�ȭ
            currentDist = 0f;       // ���� �Ÿ�
            closeDistIndex = 0;      // Ÿ�� �Ÿ�
            targetIndex = -1;       // Ÿ�� index

            for (int i = 0; i < CurrentRoomData.monsterListInROOM.Count; i++)
            {
                // i���� ���Ϳ� ���� �Ÿ�
                currentDist = Vector3.Distance(transform.position, CurrentRoomData.monsterListInROOM[i].transform.position);

                // �÷��̾� - ���� ����ĳ��Ʈ�� �浹 -> ��ֹ� �浹
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, CurrentRoomData.monsterListInROOM[i].transform.position - transform.position,
                    out hit, 20f, layerMask);

                if(isHit && hit.transform.CompareTag("Enemy"))
                {
                    // ���������� ������ ����� �Ÿ��� ������Ʈ�� Ÿ������
                    if(targetDist >= currentDist)
                    {
                        targetIndex = i;
                        targetDist = currentDist;
                    }    
                }

                // ��ֹ��� ������� ���� ����� ���� index
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
        // Ÿ���� �����ϰ�, �����̰��ִ� ���°� �ƴҶ�
        if (getATarget && !playerJoystick.IsMoveing)
        {
            float rotationX = CurrentRoomData.monsterListInROOM[targetIndex].transform.position.x;
            float rotationY = transform.position.y;
            float rotationZ = CurrentRoomData.monsterListInROOM[targetIndex].transform.position.z;

            transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));     // ���� ���� �ٶ󺸱�

            playerMovement.ChangeAnimationState(PlayerAnimatorState.ATTACK);    // ���� �ִϸ��̼� ����
        }
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        // multiAttack ��ų ���� Ƚ�� + �⺻ ���� Ƚ�� 1
        int attackCount = PlayerManager.Instance.PlayerStat.skills[(int)SkillName.MultiAttack] + 1;

        for (int i = 0; i < attackCount; i++)
        {
            AttackDelegatechain();
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    // ���� �⺻
    private void DefaultAttack()
    {
        playerMovement.PlayerAnimator.SetFloat("AtkSpeed", PlayerManager.Instance.PlayerStat.atkSpeed);
        Instantiate(playerBullet, attackPoint.position, transform.rotation, attackPoint);
    }

    // ���� ȭ�� �߰�
    private void AddStraightAttack()
    {
        int bulletCount = PlayerManager.Instance.PlayerStat.skills[(int)SkillName.AddStraight];
        float startPositionX = bulletCount / 2 * -0.30f;

        for (int i = 0; i < bulletCount; i++)
        {
            if (-0.01f <= startPositionX && startPositionX <= 0.01f)
            {
                i--;
                startPositionX += 0.30f;
                continue;
            }

            Instantiate(playerBullet, attackPoint.position, transform.rotation, attackPoint).transform.localPosition
                += new Vector3(startPositionX, 0, 0);

            startPositionX += 0.30f;
        }
    }

    // �缱 ȭ�� �߰�
    private void AddDiagonalAttack()
    {
        int bulletCount = PlayerManager.Instance.PlayerStat.skills[(int)SkillName.AddDiagonal];

        float rotationY_1 = 90f;
        float rotationY_2 = 45f;

        for(int i = 0; i < bulletCount; i++)
        {
            Instantiate(playerBullet, attackPoint.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, rotationY_1, 0)), attackPoint);
            Instantiate(playerBullet, attackPoint.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, rotationY_2, 0)), attackPoint);
            Instantiate(playerBullet, attackPoint.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -rotationY_1, 0)), attackPoint);
            Instantiate(playerBullet, attackPoint.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -rotationY_2, 0)), attackPoint);

            rotationY_1 -= 5;
            rotationY_2 -= 5;
        }
    }
}
