﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Zombie : Actor
{
    public Transform target;
    NavMeshAgent agent;
    float originalSpeed;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        attackCollider = GetComponentInChildren<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        animator = GetComponentInChildren<Animator>();

        CurrentFsm = ChaseFSM;
        while (true) // 상태를 무한히 반복해서 실행하는 부분.
        {
            var previousFSM = CurrentFsm;

            fsmHandle = StartCoroutine(CurrentFsm());

            // FSM 안에서 에러 발생시 무한 루프 도는 것을 방지 하기 위해서 추가함.
            if (fsmHandle == null && previousFSM == CurrentFsm)
                yield return null;

            while (fsmHandle != null)
                yield return null;
        }
    }
    Coroutine fsmHandle;
    protected Func<IEnumerator> CurrentFsm
    {
        get { return m_currentFsm; }
        set
        {
            if(fsmHandle != null)
                StopCoroutine(fsmHandle);   // 새로운 코루틴 함수가 시작되면 했던 코루틴을 멈춰주자 이걸 넣어주지 않으면 기존 코루틴 함수가 계속 시작됨
            m_currentFsm = value;
            fsmHandle = null;
        }
    }
    Func<IEnumerator> m_currentFsm;

    IEnumerator ChaseFSM()
    {
        if (target)
        {
            agent.destination = target.position;
        }
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        // 타겟이 공격 범위 안에 들어왔는가?
        if (TargetIsInAttackArea())
        {
            CurrentFsm = AttackFSM;
        }
        else
        {
            CurrentFsm = ChaseFSM; // FSM은 yield문이랑 다음 행동의 FSM을 지정해주어야한다
        }
    }
    public float attackTime = 0.4f;
    public float attackAnimationTime = 0.8f;
    public SphereCollider attackCollider;
    public LayerMask enemyLayer;
    public int power = 20;
    private IEnumerator AttackFSM()
    {
        yield return null;
        // 타겟 바라보기
        var lookAtPosition = target.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);

        // 공격 애니메이션 하기.
        animator.SetTrigger("Attack");
        // 공격하는 중에는 이동 스피드 0으로 하기
        agent.speed = 0;
        // 공격 타이밍까지 대기 (특정 시간 지나면)
        yield return new WaitForSeconds(attackTime);
        // 특정 시간 지나면 충돌메시 사용해서 충돌 감지하기.
        Collider[] enemyColliders = Physics.OverlapSphere(attackCollider.transform.position, 
            attackCollider.radius, enemyLayer);
        foreach (var item in enemyColliders)
        {
            item.GetComponent<Player>().TakeHit(power);
        }
        // 공격 애니메이션 끝날 때 까지 대기
        yield return new WaitForSeconds(attackAnimationTime - attackTime);
        // 이동 스피드 복구
        SetOriginalSpeed();
        // FSM지정.
        CurrentFsm = ChaseFSM;
    }

    private bool TargetIsInAttackArea()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < attackDistance;
    }

    public float attackDistance = 3;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    internal void TakeHit(int damage, Vector3 toMoveDirection)
    {
        hp -= damage;
        // 뒤로 밀려나게 하자.
        PushBackMove(toMoveDirection);
        CurrentFsm = TakeHitFSM;
        // 피격 이펙트 생성
    }

    private IEnumerator TakeHitFSM()
    {
        animator.Play(Random.Range(0, 2) == 0 ? "TakeHit1" : "TakeHit2", 0, 0);
        CreateBloodEffect();// 피 이펙트 생성
        // 이동 스피드를 잠시 0으로 만들자.
        agent.speed = 0;

        yield return new WaitForSeconds(TakeHitStopSpeedTime);
        SetOriginalSpeed();

        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(1f);
            Die();
        }
        CurrentFsm = ChaseFSM;
    }

    public float moveBackDistance = 0.1f;
    public float moveBackNoise = 0.1f;
    private void PushBackMove(Vector3 toMoveDirection)
    {
        toMoveDirection.x += Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.z += Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.y = 0;
        toMoveDirection.Normalize();
        transform.Translate(toMoveDirection * moveBackDistance, Space.World);
    }

    public float TakeHitStopSpeedTime = 0.1f;
    private void SetOriginalSpeed()
    {
        agent.speed = originalSpeed;
    }

    void Die()
    {
        animator.Play("Die");
        Destroy(gameObject, 1);
    }

}
