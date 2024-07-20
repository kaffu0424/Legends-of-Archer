using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Duck,
    Penguin,
    Sheep,
    Cat
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

    [SerializeField] private GameObject hitUI;
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

    public void Hit(float damage, Transform pos)
    {
        Instantiate(hitUI, pos.position + new Vector3(0,2,0), Quaternion.identity).GetComponent<HitUI>().DamagePopup(damage);
    }
}
