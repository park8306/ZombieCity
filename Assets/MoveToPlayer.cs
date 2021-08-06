using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;

public class MoveToPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    public float maxSpeed = 20;
    public float duration = 3; // 3초동안 최대 20의 속도로 증가

    bool alreadyDone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyDone)
        {
            return;
        }
        if(other.CompareTag("Player"))
        {
            alreadyDone = true;
            agent = GetComponent<NavMeshAgent>();
            DOTween.To(() => agent.speed, (x) => agent.speed = x, maxSpeed, duration).SetLink(gameObject);// 연결되어있는 정보를 설정 해주자?
            setDestinationCoHandle = StartCoroutine(SetDestinationCo(other.transform));
        }
    }
    public void StopCoroutine()
    {
        StopCoroutine(setDestinationCoHandle);
    }
    Coroutine setDestinationCoHandle;
    private IEnumerator SetDestinationCo(Transform tr)
    {
        while (true)
        {
            agent.destination = tr.position;
            yield return null;
        }
    }
}
