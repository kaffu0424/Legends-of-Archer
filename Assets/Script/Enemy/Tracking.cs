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
        enemy.Animator.SetBool("Tracking", true);
    }

    public override void Excute()
    {
        enemy.Tracking();
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Tracking", false);
    }
}
