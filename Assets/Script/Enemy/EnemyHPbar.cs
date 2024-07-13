using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbar : MonoBehaviour
{
    // Hpbar Offset
    Vector3 offsetRotation;

    [SerializeField] private Slider enemyHpbar;
    [SerializeField] private TextMeshProUGUI hpTEXT;
    void Start()
    {
        // 초기화
        offsetRotation = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        FixRotation();
    }

    private void FixRotation()
    {
        // Rotation값 고정시키기
        transform.rotation = Quaternion.Euler(offsetRotation);
    }

    public void UpdateHPbar(float current, float max)
    {
        enemyHpbar.value = current / max;
        hpTEXT.text = ((int)current).ToString();
    }
}
