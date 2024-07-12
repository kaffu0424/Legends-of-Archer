using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Duck,
}

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private GameObject enemyBullet;
    protected override void InitManager()
    {
        
    }

    public GameObject GetEnemy(EnemyName enemyName)
    {
        return enemyPrefabs[(int)enemyName];
    }

    public GameObject GetEnemyBullet()
    {
        return enemyBullet;
    }
}
