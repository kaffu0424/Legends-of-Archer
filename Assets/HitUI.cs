using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitUI : MonoBehaviour
{
    public TextMeshProUGUI damageTEXT;
    public void DamagePopup(float value)
    {
        damageTEXT.text = "-" + ((int)value).ToString();
        Destroy(gameObject, 1f);
    }
}
