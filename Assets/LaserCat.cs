using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCat : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private GameObject laser;

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = laser.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if(lineRenderer.enabled)
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, hitLayer);

            if(hit.transform.CompareTag("Player") || hit.transform.CompareTag("Obstacle"))
            {
                hitEffect.Play();

                hitEffect.transform.position = hit.point;

                if (hit.transform.CompareTag("Player"))
                {
                    PlayerManager.Instance.PlayerStat.GetDamage(0.1f);
                }
            }
        }
    }
}
