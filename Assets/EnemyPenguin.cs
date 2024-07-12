using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPenguin : Enemy
{
    [SerializeField] private bool canAttack;
    private bool lookon;

    [SerializeField] private Transform bulletPosition;
    [SerializeField] private LayerMask bulletLayer;
    private LineRenderer lineRenderer;

    protected override void InitializeEnemy()
    {
        enemyStat = new EnemyStat(80);  // 체력 초기화
        enemyStat.trackingRange = 12.5f;  // 탐색 범위
        enemyStat.attackRange = 12.5f;  // 공격 범위

        enemyStat.attackDelay = 3f;     // 공격 쿨타임 3초

        navMeshAgent.stoppingDistance = enemyStat.attackRange;  // 공격 범위에서 멈추는 값
        navMeshAgent.speed = enemyStat.moveSpeed;               // 이동속도 설정

        canAttack = true;

        hpbar.UpdateHPbar(enemyStat.enemyCurrentHP, enemyStat.enemyMaxHP);

        // Line Renderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor   = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth   = 0.2f;

        lookon = true;
    }

    #region 경고선 함수

    private void DangerMarkerShoot()
    {
        Vector3 newPosition = bulletPosition.position;
        Vector3 newDir = transform.forward;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, bulletPosition.position);

        for(int i = 1; i < 4; i++)
        {
            Physics.Raycast(newPosition, newDir, out RaycastHit hit, 30f, bulletLayer);

            lineRenderer.positionCount++;

            lineRenderer.SetPosition(i, hit.point);

            newPosition = hit.point;
            newDir = Vector3.Reflect(newDir, hit.normal);
        }
    }

    private void DangerMarkerDeactive()
    {
        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        lineRenderer.positionCount = 0;
    }

    IEnumerator DangerAttack()
    {
        lookon = true;

        yield return new WaitForSeconds(0.2f);  // 0.2초 대기후
        StartCoroutine(DangerMarkerUpdate());   // 경고선 출력 / 업데이트

        yield return new WaitForSeconds(2f);    // 2초 대기후
        ShootBullet();
        lookon = false;

        yield return new WaitForSeconds(0.1f);  // 0.1초 대기후

        // 공격 쿨타임
        yield return new WaitForSeconds(enemyStat.attackDelay);
        canAttack = true;
    }

    IEnumerator DangerMarkerUpdate()
    {
        while(lookon)
        {
            yield return null;
            DangerMarkerShoot();        // 경고선 출력 및 업데이트
        }

        DangerMarkerDeactive();                 // 경고선 제거
    }
    #endregion

    private void ShootBullet()
    {
        Instantiate(EnemyManager.Instance.GetEnemyBullet(), bulletPosition.position, transform.rotation);
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
            float rotationX = trackingTarget.position.x;
            float rotationY = transform.position.y;
            float rotationZ = trackingTarget.position.z;

            transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));

            // 공격 범위에 플레이어가 있을때
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // 공격 가능한 상태일때
                if(canAttack)
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
