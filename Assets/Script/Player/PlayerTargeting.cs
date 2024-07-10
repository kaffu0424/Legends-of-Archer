using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
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
        playerMovement.PlayerAnimator.SetFloat("AtkSpeed", PlayerManager.Instance.PlayerStat.atkSpeed);
        Instantiate(playerBullet, attackPoint.position, transform.rotation);
    }
}
