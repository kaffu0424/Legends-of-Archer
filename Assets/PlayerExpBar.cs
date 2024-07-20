using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    Slider expbar;
    [SerializeField] TextMeshProUGUI levelTEXT;
    private void Start()
    {
        expbar = GetComponent<Slider>();
        levelTEXT = GetComponentInChildren<TextMeshProUGUI>();

        levelTEXT.text = "Lv. 1";
        expbar.value = 0;
    }

    public void UpdateExpBar(ref PlayerStat stat)
    {
        expbar.value = stat.currentEXP / stat.maxEXP;
        levelTEXT.text = "Lv. " + stat.level.ToString();
    }
}
