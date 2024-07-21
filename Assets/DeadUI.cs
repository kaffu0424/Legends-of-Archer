using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deadtext;
    [SerializeField] private TextMeshProUGUI stageText;


    public void DeadUIUpdate()
    {
        if(PlayerManager.Instance.PlayerStat.currentHP <= 0)
        {
            deadtext.text = "YOU DIED";
            stageText.text = "STAGE : " + (StageManager.Instance.stage + 1);
        }
        else
        {
            deadtext.text = "GAME CLEAR";
            stageText.text = "STAGE : " + StageManager.Instance.stage;
        }
    }
}
