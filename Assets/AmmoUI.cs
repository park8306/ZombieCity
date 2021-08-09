using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : GaugeUI<AmmoUI>
{
    internal void SetBulletCount(int bulletCountInClip, int maxBulletCountClip, int allBulletCount, int maxBulletCount)
    {
        SetGauge(bulletCountInClip, maxBulletCountClip);
        valueText.text = $"{allBulletCount}/{maxBulletCount}";
    }

    internal void StartReload(int bulletCountInClip, int maxBulletCountClip, int allBulletCount, int maxBulletCount, float duration)
    {
        StartCoroutine(SetAnimateGaugeCo(bulletCountInClip, maxBulletCountClip, duration));
        valueText.text = $"{allBulletCount}/{maxBulletCount}";
    }
}
