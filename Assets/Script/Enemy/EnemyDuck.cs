using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDuck : Enemy
{
    [SerializeField] private bool canAttack;

    [SerializeField] private ParticleSystem attackEffect;
    protected override void InitializeEnemy()
    {
        enemyStat = new EnemyStat(100);     // 체력 초기화
        enemyStat.trackingRange = 10f;      // 탐색 범위
        enemyStat.attackRange = 2f;         // 공격 범위

        navMeshAgent.stoppingDistance = enemyStat.attackRange;  // 공격 범위에서 멈추는 값
        navMeshAgent.speed = enemyStat.moveSpeed;               // 이동 속도 설정

        canAttack = true;

        hpbar.UpdateHPbar(enemyStat.enemyCurrentHP, enemyStat.enemyMaxHP);
    }
    public override void Attack()
    {
        if(canAttack)
        {
            // 공격 쿨타임
            StartCoroutine(AttackDelay());

            // 공격 기능
            attackEffect.Play();    // 공격 이펙트
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStat.attackRange, targetLayer);
            foreach(Collider collider in colliders)
            {
                if(collider.CompareTag("Player"))
                {
                    // 몬스터 공격력 스탯 추가하기.
                    PlayerManager.Instance.PlayerStat.GetDamage(100);
                }
            }

        }

        else
        {
            // 공격 쿨타임중 Idle 상태로 전이
            ChangeState(EnemyState.Idle);
        }
    }

    public override void Idle()
    {
        // 플레이어 탐색
        if(FindPlayer())
        {
            // Idle 중 공격 범위 내 플레이어가 있을때,
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // 공격이 가능하면 공격 상태로 전이
                if(canAttack)
                    ChangeState(EnemyState.Attack);
            }
            else
            {
                ChangeState(EnemyState.Tracking);   // Tracking 상태 전이
            }
        }
    }

    public override void Tracking()
    {
        // Tracking 범위 내 플레이어가 있으면 추격
        if(FindPlayer())
        {
            // 이동
            navMeshAgent.SetDestination(trackingTarget.position);

            // 추격 중 공격 범위 내 플레이어가 있고,
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // 공격 가능한 상태일때
                if(canAttack)
                    ChangeState(EnemyState.Attack);
            }    
        }

        // Tracking 범위 내 플레이어가 없으면 Idle 상태로 전이
        else
        {
            ChangeState(EnemyState.Idle);
        }
    }

    IEnumerator AttackDelay()
    {
        // 공격 불가능
        canAttack = false;

        // 공격 쿨타임 만큼 대기
        yield return new WaitForSeconds(enemyStat.attackDelay);

        // 공격 가능 
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            GetDamage(PlayerManager.Instance.PlayerStat.damage);
        }
    }
}
