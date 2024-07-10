using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Duck,
}

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] List<GameObject> enemyPrefabs;

    protected override void InitManager()
    {
        
    }

    public GameObject GetEnemy(EnemyName enemyName)
    {
        return enemyPrefabs[(int)enemyName];
    }
}
