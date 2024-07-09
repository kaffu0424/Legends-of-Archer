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

public class EnemyStat
{
    public float enemyMaxHP;
    public float enemyCurrentHP;

    public EnemyStat(float enemyHP)
    {
        enemyMaxHP = enemyHP;
        enemyCurrentHP = enemyHP;
    }
}

public class Enemy : MonoBehaviour
{
    // FSM
    protected EnemyFSM currentFSM;
    protected EnemyFSM[] FSMs;
    private EnemyState enemyState;

    // components
    protected NavMeshAgent navMeshAgent;

    private void Start()
    {
        // FSM 초기화
        FSMs = new EnemyFSM[Enum.GetValues(typeof(EnemyState)).Length];
        FSMs[(int)EnemyState.Idle]      = new Idle(this);
        FSMs[(int)EnemyState.Tracking]  = new Tracking(this);
        FSMs[(int)EnemyState.Attack]    = new Attack(this);
        
        // 초기 상태 변경
        ChangeState(EnemyState.Idle);
    }
    private void Update()
    {
        currentFSM.Excute();

        if(Input.GetKeyDown(KeyCode.I)) 
        {
            ChangeState(EnemyState.Idle);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeState(EnemyState.Tracking);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(EnemyState.Attack);
        }
    }

    private void ChangeState(EnemyState state)
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
}
