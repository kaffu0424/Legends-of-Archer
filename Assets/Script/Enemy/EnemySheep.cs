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
        enemyStat = new EnemyStat(150);  // ü�� �ʱ�ȭ
        enemyStat.trackingRange = 15f;  // Ž�� ����
        enemyStat.attackRange = 15f;  // ���� ����

        enemyStat.attackDelay = 2.5f;     // ���� ��Ÿ�� 3��

        //navMeshAgent.stoppingDistance = enemyStat.attackRange;  // ���� �������� ���ߴ� ��
        //navMeshAgent.speed = enemyStat.moveSpeed;               // �̵��ӵ� ����

        canAttack = true;

        hpbar.UpdateHPbar(enemyStat.enemyCurrentHP, enemyStat.enemyMaxHP);

        lookon = true;
    }

    private void DangerMarkerShoot()
    {
        // ��� ���� ��ġ
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
        lookon = false;     // �÷��̾� �ٶ󺸱� ����

        DangerMarkerShoot(); // ��� ���
        yield return new WaitForSeconds(2f);    // ���

        ShootBullet();
        yield return new WaitForSeconds(0.1f);  // ���
        dangerLine.ClearDangerLine();

        lookon = true;
        // ���� ��Ÿ��
        yield return new WaitForSeconds(enemyStat.attackDelay);
        canAttack = true;
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
            if(lookon)
            {
                float rotationX = trackingTarget.position.x;
                float rotationY = transform.position.y;
                float rotationZ = trackingTarget.position.z;

                transform.LookAt(new Vector3(rotationX, rotationY, rotationZ));
            }

            // ���� ������ �÷��̾ ������
            if (Vector3.Distance(trackingTarget.position, transform.position) <= enemyStat.attackRange)
            {
                // ���� ������ �����϶�
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
