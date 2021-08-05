using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class MoveToPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    public float maxSpeed = 20;
    public float duration = 3; // 3초동안 최대 20의 속도로 증가

    bool alreadyDone = false;
    TweenerCore<float, float, FloatOptions> tweenResult;
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (alreadyDone)
        {
            yield break;
        }
        if(other.CompareTag("Player"))
        {
            alreadyDone = true;
            agent = GetComponent<NavMeshAgent>();
            tweenResult = DOTween.To(() => agent.speed, (x) => agent.speed = x, maxSpeed, duration);    // getter 초기값

            while (true) // 코인의 목표지점을 계속 지정해줌
            {
                agent.destination = other.transform.position;
                yield return null;
            }
        }
    }
    private void OnDestroy()
    {
        tweenResult.Kill();
    }
}
