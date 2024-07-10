using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,       // 멈춤
    Tracking,   // 추적
    Attack      // 공격
}

[System.Serializable]
public class EnemyStat
{
    // HP
    public float enemyMaxHP;        // 최대 체력
    public float enemyCurrentHP;    // 현재 체력

    // Speed
    public float moveSpeed;         // 이동속도
    public float attackDelay;       // 공격 쿨타임

    // Range
    public float trackingRange;      // 추격 범위 - 해당 범위 내 플레이어가 있으면 추격 ( 없으면 idle )
    public float attackRange;       // 공격 범위 - 해당 범위 내 플레이어가 있으면 공격 ( 없으면 tracking or idle )

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
    protected EnemyStat enemyStat;

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

    // GET / SET
    public Animator Animator => animator;
    private void Start()
    {
        // FSM 초기화
        FSMs = new EnemyFSM[Enum.GetValues(typeof(EnemyState)).Length];
        FSMs[(int)EnemyState.Idle]      = new Idle(this);
        FSMs[(int)EnemyState.Tracking]  = new Tracking(this);
        FSMs[(int)EnemyState.Attack]    = new Attack(this);
        
        // Component
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // 몬스터 초기화 함수
        InitializeEnemy();

        // 초기 상태 변경
        ChangeState(EnemyState.Idle);

    }
    private void Update()
    {
        currentFSM.Excute();
    }

    protected void ChangeState(EnemyState state)
    {
        // 1. 기존 상태 퇴장시
        FSMs[(int)enemyState].Exit();

        // 2. 상태 변경
        enemyState = state;

        // 3. 새로운 상태 진입
        FSMs[(int)enemyState].Enter();

        // 4.현재 상태 FSM 초기화
        currentFSM = FSMs[(int)enemyState];
    }

    protected abstract void InitializeEnemy();
    public abstract void Idle();
    public abstract void Tracking();
    public abstract void Attack();

    protected bool FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStat.trackingRange, targetLayer);  // 범위 확인
        foreach (Collider target in colliders)
        {
            trackingTarget = target.transform;              // 감지된 오브젝트를 타겟으로 설정
            return true;
        }
        trackingTarget = null;                         // 오브젝트가 감지되지않았을때 타겟을 null
        return false;
    }
}
