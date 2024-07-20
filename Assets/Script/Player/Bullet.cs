using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    Vector3 newDir;

    int enemyBounce;
    int wallBounce;

    [SerializeField] private LayerMask enemyLayer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        newDir = transform.forward;
        rb.velocity = transform.forward * 10;

        enemyBounce = PlayerManager.Instance.PlayerStat.skills[(int)SkillName.EnemyBounce];
        wallBounce = PlayerManager.Instance.PlayerStat.skills[(int)SkillName.Bounce];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if(wallBounce > 0)
            {
                wallBounce--;
                newDir = Vector3.Reflect(newDir, collision.contacts[0].normal);
                rb.velocity = newDir * 10f;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if(enemyBounce > 0)
            {
                enemyBounce--;

                Collider[] collider = Physics.OverlapSphere(transform.position, 5f, enemyLayer);

                Collider Target = null;
                float targetDistance = 500f;
                
                foreach(Collider col in collider)
                {
                    if (col.gameObject == collision.gameObject)
                        continue;

                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance <= targetDistance)
                    {
                        Target = col;
                        targetDistance = distance;
                    }
                }

                if(Target == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    newDir = ResultDir(Target.transform) * 20f;
                    rb.velocity = newDir;
                }
            }
            else
            {
                Destroy(gameObject);
            }    
        }
    }

    private Vector3 ResultDir(Transform target)
    {
        return (target.position - transform.position).normalized;
    }
}
