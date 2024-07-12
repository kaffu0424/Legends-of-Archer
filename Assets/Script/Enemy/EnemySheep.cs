using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySheep : Enemy
{
    [SerializeField] private bool canAttack;
    private bool lookon;

    [SerializeField] private Transform bulletPosition;
    [SerializeField] private DangerLine dangerLine;

    [SerializeField] private LayerMask bulletLayer;

    protected override void InitializeEnemy()
    {
        enemyStat = new EnemyStat(150);  // 체력 초기화
        enemyStat.trackingRange = 15f;  // 탐색 범위
        enemyStat.attackRange = 15f;  // 공격 범위

        enemyStat.attackDelay = 2.5f;     // 공격 쿨타임 3초

        //navMeshAgent.stoppingDistance = enemyStat.attackRange;  // 공격 범위에서 멈추는 값
        //navMeshAgent.speed = enemyStat.moveSpeed;               // 이동속도 설정

        canAttack = true;

        hpbar.UpdateHPbar(enemyStat.enemyCurrentHP, enemyStat.enemyMaxHP);

        lookon = true;
    }

    private void DangerMarkerShoot()
    {
        // 경고선 시작 위치
        Vector3 marketStartPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Physics.Raycast(marketStartPos, transform.forward, out RaycastHit hit, 30f, bulletLayer);

        if(hit.transform.CompareTag("Obstacle"))
        {
            dangerLine.ShowDangerLine(hit.point);
        }
    }
    
    private void ShootBullet()
    {
        Instantiate(EnemyManager.Instance.GetEnemyBullet(BulletType.One), bulletPosition.position, transform.rotation);
    }

    IEnumerator DangerAttack()
    {
        yield return null;
        lookon = false;     // 플레이어 바라보기 방지

        DangerMarkerShoot(); // 경고선 출력
        yield return new WaitForSeconds(2f);    // 대기

        ShootBullet();
        yield return new WaitForSeconds(0.1f);  // 대기
        dangerLine.ClearDangerLine();

        lookon = true;
        // 공격 쿨타임
        yield return new WaitForSeconds(enemyStat.attackDelay);
        canAttack = true;
    }

    public override void Attack()
    {
        
    }

    public override void Idle()
    {
        // 플레이어가 탐색범위에 있을때
        if (FindPlayer())
        {
            // 플레이어 계속 바라보기
            if(lookon)
            {
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
                    StartCoroutine(DangerAttack());
                }
            }
        }
    }

    public override void Tracking()
    {
        
    }

}
