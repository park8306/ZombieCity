using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MoveToPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    public float maxSpeed = 20;
    public float duration = 3; // 3초동안 최대 20의 속도로 증가
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            agent = GetComponent<NavMeshAgent>();
            DOTween.To(() => agent.speed, (x) => agent.speed = x, maxSpeed, duration);    // getter 초기값

            while (true) // 코인의 목표지점을 계속 지정해줌
            {
                agent.destination = other.transform.position;
                yield return null;
            }
        }
    }
}
