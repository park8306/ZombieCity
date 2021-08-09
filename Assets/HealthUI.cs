using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : GaugeUI<HealthUI>
{
}
public class GaugeUI<T> : SingletonMonoBehavior<T>
where T : SingletonBase
{

    protected TextMeshProUGUI valueText;
    public Image[] images;
    public Sprite enable, current, disable;  // enable : 사용가능, current : 현재 사용, disable: 사용 했음

    protected override void OnInit()    // 실행 순서가 보장이 됨
    {
        valueText = transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
    }

    internal void SetGauge(int value, int maxValue)
    {
        valueText.text = $"{value}/{maxValue}";   // valueText에 현재 hp의 정보를 나타냄

        float percent = (float)value / maxValue;

        int currentCount = Mathf.RoundToInt(percent * images.Length - 1);

        for (int i = 0; i < images.Length; i++)
        {
            if (i == currentCount)
            {
                images[i].sprite = current;
            }
            else if (i < currentCount)
            {
                images[i].sprite = enable;
            }
            else
            {
                images[i].sprite = disable;
            }
        }
    }

    protected IEnumerator SetAnimateGaugeCo(int value, int maxValue, float duration)
    {
        foreach (var item in images)
        {
            item.sprite = disable;
        }
        float timePerEach = duration / images.Length;
        float percent = (float)value / maxValue;
        int currentCount = Mathf.RoundToInt(percent * images.Length) - 1;

        for (int i = 0; i < images.Length; i++)
        {
            if (i == currentCount)
            {
                images[i].sprite = current;
            }
            else if (i < currentCount)
            {
                images[i].sprite = enable;
            }
            else
            {
                images[i].sprite = disable;
            }
            yield return new WaitForSeconds(timePerEach);
        }
    }
}
