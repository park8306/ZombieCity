using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : SingletonMonoBehavior<GoldUI>
{
    TextMeshProUGUI goldText;
    protected override void OnInit()
    {
        goldText = transform.Find("GoldText").GetComponent<TextMeshProUGUI>();
    }

    internal void UpdateUI(int gold)
    {
        goldText.text = $"Gold:{gold.ToNumber()}";
    }
}
