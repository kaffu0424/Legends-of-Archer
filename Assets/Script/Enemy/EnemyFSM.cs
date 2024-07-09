using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSM
{
    public Enemy enemy;

    public abstract void Enter();
    public abstract void Excute();
    public abstract void Exit();
}
