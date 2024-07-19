using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }

        else if(other.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
