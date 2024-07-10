using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPbar : MonoBehaviour
{
    private const float unitHP = 200;

    Vector3 offset;
    Transform playerTransform;
    private PlayerStat playerStat;

    [SerializeField] private Slider playerHPbar;
    [SerializeField] private TextMeshProUGUI playerHPTEXT;
    [SerializeField] private HorizontalLayoutGroup lineHPLayoutGroupComponent;

    private void Start()
    {
        offset = transform.position;
        playerTransform = PlayerManager.Instance.playerTransform;
        playerStat = PlayerManager.Instance.PlayerStat;
    }

    private void LateUpdate()
    {
        HPbarMove();
        HPbarUpdate();
    }

    private void HPbarMove()
    {
        transform.position = playerTransform.position + offset;
    }

    private void HPbarUpdate()
    {
        playerHPbar.value = playerStat.currentHP / playerStat.maxHP;// 체력 게이지
        playerHPTEXT.text = ((int)playerStat.currentHP).ToString(); // 체력 텍스트
    }

    public void UpdateMaxHP()
    {
        float scaleX = (1000f / unitHP) / ((float)playerStat.maxHP / unitHP);

        lineHPLayoutGroupComponent.gameObject.SetActive(false);
        foreach(Transform child in lineHPLayoutGroupComponent.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }

        lineHPLayoutGroupComponent.gameObject.SetActive(true);
    }
}
