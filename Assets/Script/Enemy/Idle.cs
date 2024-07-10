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
        enemy.Animator.SetBool("Idle", true);
        enemy.Animator.SetBool("Tracking", false);
        enemy.Animator.SetBool("Attack", false);
    }

    public override void Excute()
    {
        enemy.Idle();
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Idle", false);
    }
}
