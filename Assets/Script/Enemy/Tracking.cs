using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : EnemyFSM
{
    public Tracking(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("Tracking enter");
    }

    public override void Excute()
    {
        Debug.Log("Tracking Excute");
        Debug.Log("name : " + enemy.gameObject.name);
    }

    public override void Exit()
    {
        Debug.Log("Tracking Exit");
    }
}
