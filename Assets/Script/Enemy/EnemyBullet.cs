using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody rb;
    Vector3 newDir;

    int bounceCnt;

    [SerializeField] private BulletType type;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        newDir = transform.forward;
        rb.velocity = transform.forward * 10;

        switch(type)
        {
            case BulletType.One:
                bounceCnt = 0;
                break;
            case BulletType.Three:
                bounceCnt = 3;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Obstacle"))
        {
            bounceCnt--;
            if(bounceCnt > 0)
            {
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                rb.velocity = newDir * 10;
            }
            else
            {
                Destroy(gameObject, 0.05f);
            }
        }

        if(collision.transform.CompareTag("Player"))
        {
            PlayerManager.Instance.PlayerStat.GetDamage(60);
            Destroy(gameObject, 0.05f);
        }
    }
}