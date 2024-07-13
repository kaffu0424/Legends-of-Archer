using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    private TrailRenderer trail;
    private Vector3 defaultPosition;
    private Vector3 targetPosition;
    private bool onMove;
    void Start()
    {
        trail = GetComponent<TrailRenderer>();

        trail.startColor = new Color(1, 0, 0, 0.7f);
        trail.endColor = new Color(1, 0, 0, 0.7f);

        trail.enabled = false;

        defaultPosition = transform.position;

        onMove = false;
    }

    private void Update()
    {
        if(onMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3.5f);
        }
    }

    public void ShowDangerLine(Vector3 endPosition)
    {
        onMove = true;
        trail.enabled = true;       // Ʈ���Ϸ� ON

        targetPosition = endPosition;
    }

    public void ClearDangerLine()
    {
        onMove = false;
        trail.enabled = false;                  // Ʈ���Ϸ� OFF
        transform.position = defaultPosition;   // ���� ��ġ�� �̵�

        trail.Clear();
    }
}
