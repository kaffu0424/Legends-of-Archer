using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyFSM
{
    public Attack(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("Attack enter");
    }

    public override void Excute()
    {
        Debug.Log("Attack Excute");
        Debug.Log("name : " + enemy.gameObject.name);
    }

    public override void Exit()
    {
        Debug.Log("Attack Exit");
    }
}
