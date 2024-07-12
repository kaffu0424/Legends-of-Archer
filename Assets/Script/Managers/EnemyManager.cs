using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Duck,
    Penguin,
    Sheep,
}

public enum BulletType
{
    One,
    Three
}

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> enemyBullets;
    protected override void InitManager()
    {
        
    }

    public GameObject GetEnemy(EnemyName enemyName)
    {
        return enemyPrefabs[(int)enemyName];
    }

    public GameObject GetEnemyBullet(BulletType type)
    {
        return enemyBullets[(int)type];
    }
}
