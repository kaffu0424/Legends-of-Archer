using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDuck : Enemy
{
    [SerializeField] private bool canAttack;

    protected override void InitializeEnemy()
    {
        enemyStat = new EnemyStat(100);     // ü�� �ʱ�ȭ
        enemyStat.trackingRange = 10f;      // Ž�� ����
        enemyStat.attackRange = 2f;         // ���� ����

        navMeshAgent.stoppingDistance = enemyStat.attackRange;  // ���� �������� ���ߴ� ��
        navMeshAgent.speed = enemyStat.moveSpeed;               // �̵� �ӵ� ����

        canAttack = true;
    }
    public override void Attack()
    {
        if(canAttack)
        {
            // ���� ��Ÿ��
            StartCoroutine(AttackDelay());

            // ���� ���
            Debug.Log("����!");
        }

        else
        {
            // ���� ��Ÿ���� Idle ���·� ����
            ChangeState(EnemyState.Idle);
        }
    }

    public override void Idle()
    {
        // �÷��̾� Ž��
        if(FindPlayer())
        {
            // Idle �� ���� ���� �� �÷��̾ ������,
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // ������ �����ϸ� ���� ���·� ����
                if(canAttack)
                    ChangeState(EnemyState.Attack);
            }
            else
            {
                ChangeState(EnemyState.Tracking);   // Tracking ���� ����
            }
        }
    }

    public override void Tracking()
    {
        // Tracking ���� �� �÷��̾ ������ �߰�
        if(FindPlayer())
        {
            // �̵�
            navMeshAgent.SetDestination(trackingTarget.position);

            // �߰� �� ���� ���� �� �÷��̾ �ְ�,
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // ���� ������ �����϶�
                if(canAttack)
                    ChangeState(EnemyState.Attack);
            }    
        }

        // Tracking ���� �� �÷��̾ ������ Idle ���·� ����
        else
        {
            ChangeState(EnemyState.Idle);
        }
    }

    IEnumerator AttackDelay()
    {
        // ���� �Ұ���
        canAttack = false;

        // ���� ��Ÿ�� ��ŭ ���
        yield return new WaitForSeconds(enemyStat.attackDelay);

        // ���� ���� 
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        if (enemyStat == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyStat.trackingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStat.attackRange);
    }
}
