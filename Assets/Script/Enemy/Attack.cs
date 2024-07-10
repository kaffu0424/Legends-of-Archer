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
        enemy.Animator.SetBool("Attack", true);
    }

    public override void Excute()
    {
        enemy.Attack();
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Attack", false);
    }
}
