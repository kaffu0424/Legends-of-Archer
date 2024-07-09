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
        // FSM �ʱ�ȭ
        FSMs = new EnemyFSM[Enum.GetValues(typeof(EnemyState)).Length];
        FSMs[(int)EnemyState.Idle]      = new Idle(this);
        FSMs[(int)EnemyState.Tracking]  = new Tracking(this);
        FSMs[(int)EnemyState.Attack]    = new Attack(this);
        
        // �ʱ� ���� ����
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
        // 1. ���� ���� �����
        FSMs[(int)enemyState].Exit();

        // 2. ���� ����
        enemyState = state;

        // 3. ���ο� ���� ����
        FSMs[(int)enemyState].Enter();

        // 4.���� ���� FSM �ʱ�ȭ
        currentFSM = FSMs[(int)enemyState];
    }
}
