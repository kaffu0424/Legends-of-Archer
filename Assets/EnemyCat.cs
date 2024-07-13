using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCat : Enemy
{
    [SerializeField] private bool canAttack;
    private bool lookon;

    [SerializeField] private GameObject laserEffect;

    private float attackTime = 5f;
    private float attackTimeCalc = 5f;
    protected override void InitializeEnemy()
    {
        enemyStat = new EnemyStat(200);
        enemyStat.trackingRange = 15f;
        enemyStat.attackRange = 15f;

        enemyStat.attackDelay = 3f;

        canAttack = true;
        lookon = true;

        hpbar.UpdateHPbar(enemyStat.enemyCurrentHP, enemyStat.enemyMaxHP);

        StartCoroutine(LaserOff());
    }

    public override void Attack()
    {
        
    }

    public override void Idle()
    {
        // 플레이어가 탐색 범위에 있을때
        if(FindPlayer())
        {
            if(lookon)
            {
                // 플레이어 바라보기
                float rotationX = trackingTarget.position.x;
                float rotationY = transform.position.y;
                float rotationZ = trackingTarget.position.z;

                transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));
            }

            // 공격 범위에 플레이어가 있을때
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // 공격 가능한 상태일때
                if (canAttack)
                {
                    canAttack = false;
                    StartCoroutine(LaserAttack());
                }
            }
        }
    }

    public override void Tracking()
    {
        
    }

    IEnumerator LaserAttack()
    {
        canAttack = false;
        lookon = false;

        laserEffect.SetActive(true);
        StartCoroutine(LaserTargeting());   // 천천히 타겟팅
        yield return new WaitForSeconds(attackTime);

        lookon = true;
        yield return new WaitForSeconds(enemyStat.attackDelay);
        canAttack = true;
    }

    IEnumerator LaserTargeting()
    {
        while(true)
        {
            yield return null;
            if (!laserEffect.activeSelf)
                break;

            Quaternion targetRotation = Quaternion.LookRotation(trackingTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        }
    }

    IEnumerator LaserOff()
    {
        while(true)
        {
            yield return null;
            // 레이저가 활성화되어있을땐
            if(laserEffect.activeSelf)  
            {
                attackTimeCalc -= Time.deltaTime;
                // 5초 후
                if(attackTimeCalc <= 0)
                {
                    // 시간 초기화 / 레이저 Effect 비활성화
                    attackTimeCalc = attackTime;
                    laserEffect.SetActive(false);
                }
            }
        }
    }
}
