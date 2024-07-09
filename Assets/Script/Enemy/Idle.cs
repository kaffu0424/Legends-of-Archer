using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : EnemyFSM
{
    public Idle(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("Idle enter");
    }

    public override void Excute()
    {
        Debug.Log("Idle Excute");
        Debug.Log("name : " + enemy.gameObject.name);
    }

    public override void Exit()
    {
        Debug.Log("Idle Exit");
    }
}
