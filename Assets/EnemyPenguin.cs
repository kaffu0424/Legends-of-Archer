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
        enemyStat = new EnemyStat(80);  // ü�� �ʱ�ȭ
        enemyStat.trackingRange = 12.5f;  // Ž�� ����
        enemyStat.attackRange = 12.5f;  // ���� ����

        enemyStat.attackDelay = 3f;     // ���� ��Ÿ�� 3��

        navMeshAgent.stoppingDistance = enemyStat.attackRange;  // ���� �������� ���ߴ� ��
        navMeshAgent.speed = enemyStat.moveSpeed;               // �̵��ӵ� ����

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

    #region ��� �Լ�

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

        yield return new WaitForSeconds(0.2f);  // 0.2�� �����
        StartCoroutine(DangerMarkerUpdate());   // ��� ��� / ������Ʈ

        yield return new WaitForSeconds(2f);    // 2�� �����
        ShootBullet();
        lookon = false;

        yield return new WaitForSeconds(0.1f);  // 0.1�� �����

        // ���� ��Ÿ��
        yield return new WaitForSeconds(enemyStat.attackDelay);
        canAttack = true;
    }

    IEnumerator DangerMarkerUpdate()
    {
        while(lookon)
        {
            yield return null;
            DangerMarkerShoot();        // ��� ��� �� ������Ʈ
        }

        DangerMarkerDeactive();                 // ��� ����
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
        // �÷��̾ Ž�������� ������
        if (FindPlayer())
        {
            // �÷��̾� ��� �ٶ󺸱�
            float rotationX = trackingTarget.position.x;
            float rotationY = transform.position.y;
            float rotationZ = trackingTarget.position.z;

            transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));

            // ���� ������ �÷��̾ ������
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // ���� ������ �����϶�
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
