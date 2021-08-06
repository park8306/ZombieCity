using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // 환경관
    public Color dayColor;
    public Color nightColor;

    [ContextMenu("SetDayLight")]
    void SetDayLight()
    {
        // 디렉셔널 라이트만 켜기
        
        var allLight = FindObjectsOfType<Light>(); // 모든 Light 타입을 찾는다
        foreach (var item in allLight)
        {
            if (item.type == LightType.Directional)
            {
                item.enabled = true;
            }
            else
            {
                item.enabled = false;
            }
        }
        RenderSettings.ambientLight = dayColor;
    }
    [ContextMenu("SetnightLight")]
    void SetnightLight()
    {
        // 디렉셔널 라이트만 끄고
        var allLight = FindObjectsOfType<Light>(); // 모든 Light 타입을 찾는다
        foreach (var item in allLight)
        {
            if (item.type == LightType.Directional)
            {
                item.enabled = false;
            }
            else
            {
                item.enabled = true;
            }
        }
        RenderSettings.ambientLight = nightColor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeDayLight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeNightLight();
        }
    }

    Dictionary<Light, float> allLight;// = new Dictionary<Light, float>();
    public float changeDuration = 3;
    private void ChangeDayLight()
    {
        if (allLight == null)
        {
            allLight = new Dictionary<Light, float>();
            var _allLight = FindObjectsOfType<Light>(); // 모든 Light 타입을 찾는다
            foreach (var item in _allLight)
            {
                allLight[item] = item.intensity;
            }
        }
        // 낮으로 변함 -> 디렉셔널 라이트 점점 밝게
        foreach (var item in allLight)
        {
            item.Key.enabled = true;
            if (item.Key.type == LightType.Directional)
            {
                DOTween.To(() => 0f, (x) => item.Key.intensity = x, item.Value, changeDuration);
            }
            else
            {
                DOTween.To(() => item.Value, (x) => item.Key.intensity = x, 0, changeDuration);
            }
        }
        DOTween.To(() => Camera.main.backgroundColor, (x) => Camera.main.backgroundColor = x, dayColor, changeDuration);
    }
    private void ChangeNightLight()
    {
        if (allLight == null)
        {
            allLight = new Dictionary<Light, float>();
            var _allLight = FindObjectsOfType<Light>(); // 모든 Light 타입을 찾는다
            foreach (var item in _allLight)
            {
                allLight[item] = item.intensity;
            }
        }
        // 낮으로 변함 -> 디렉셔널 라이트 점점 밝게
        foreach (var item in allLight)
        {
            item.Key.enabled = true;
            if (item.Key.type != LightType.Directional)
            {
                DOTween.To(() => 0f, (x) => item.Key.intensity = x, item.Value, changeDuration);
            }
            else
            {
                DOTween.To(() => item.Value, (x) => item.Key.intensity = x, 0, changeDuration);
            }
        }
        DOTween.To(() => Camera.main.backgroundColor, (x) => Camera.main.backgroundColor = x, nightColor, changeDuration);
    }
}

