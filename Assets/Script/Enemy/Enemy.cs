using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,       // ����
    Tracking,   // ����
    Attack      // ����
}

[System.Serializable]
public class EnemyStat
{
    // HP
    public float enemyMaxHP;        // �ִ� ü��
    public float enemyCurrentHP;    // ���� ü��

    // Speed
    public float moveSpeed;         // �̵��ӵ�
    public float attackDelay;       // ���� ��Ÿ��

    // Range
    public float trackingRange;      // �߰� ���� - �ش� ���� �� �÷��̾ ������ �߰� ( ������ idle )
    public float attackRange;       // ���� ���� - �ش� ���� �� �÷��̾ ������ ���� ( ������ tracking or idle )

    public EnemyStat(float _enemyHP)
    {
        enemyMaxHP = _enemyHP;
        enemyCurrentHP = _enemyHP;

        moveSpeed = 2.5f;
        attackDelay = 1.5f;

        trackingRange = 15f;
        attackRange = 2f;
    }
}

public abstract class Enemy : MonoBehaviour
{
    //Stat
    [SerializeField] protected EnemyStat enemyStat;

    // Tracking
    [SerializeField] protected LayerMask targetLayer;
    protected Transform trackingTarget;

    // FSM
    protected EnemyFSM currentFSM;
    protected EnemyFSM[] FSMs;
    private EnemyState enemyState;

    // components
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    // RoomData
    public Room currentRoom;
    
    // GET / SET
    public Animator Animator => animator;
    private void Start()
    {
        // FSM �ʱ�ȭ
        FSMs = new EnemyFSM[Enum.GetValues(typeof(EnemyState)).Length];
        FSMs[(int)EnemyState.Idle]      = new Idle(this);
        FSMs[(int)EnemyState.Tracking]  = new Tracking(this);
        FSMs[(int)EnemyState.Attack]    = new Attack(this);
        
        // Component
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // ���� �ʱ�ȭ �Լ�
        InitializeEnemy();

        // �ʱ� ���� ����
        ChangeState(EnemyState.Idle);

    }
    private void Update()
    {
        currentFSM.Excute();
    }

    protected void ChangeState(EnemyState state)
    {
        // 1. ���� ���� �����
        FSMs[(int)enemyState].Exit();

        // 2. ���� ����
        enemyState = state;

        // 3. ���ο� ���� ����
        FSMs[(int)enemyState].Enter();

        // 4.���� ���� FSM �ʱ�ȭ
        currentFSM = FSMs[(int)enemyState];
    }

    protected abstract void InitializeEnemy();
    public abstract void Idle();
    public abstract void Tracking();
    public abstract void Attack();

    protected bool FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStat.trackingRange, targetLayer);  // ���� Ȯ��
        foreach (Collider target in colliders)
        {
            trackingTarget = target.transform;              // ������ ������Ʈ�� Ÿ������ ����
            return true;
        }
        trackingTarget = null;                         // ������Ʈ�� ���������ʾ����� Ÿ���� null
        return false;
    }

    public void GetDamage(float value)
    {
        enemyStat.enemyCurrentHP -= value;

        if(enemyStat.enemyCurrentHP <= 0 )
        {
            enemyStat.enemyCurrentHP = 0;

            // ��� ó��
            currentRoom.RemoveEnemy(this.gameObject);
            Destroy(this.gameObject, 0.05f);
        }
    }
}
