using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    public int hp = 100;
    float originalSpeed;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        animator = GetComponentInChildren<Animator>();
        //while (hp > 0)
        //{
        //    if (target)
        //    {
        //        agent.destination = target.position;
        //    }
        //    yield return new WaitForSeconds(Random.Range(0, 0.5f));
        //}

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
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        CurrentFsm = ChaseFSM; // FSM은 yield문이랑 다음 행동의 FSM을 지정해주어야한다
    }

    public float bloodEffectYPosition = 1.3f;
    public GameObject bloodParticle;
    private void CreateBloodEffect()
    {
        var pos = transform.position;
        pos.y = bloodEffectYPosition;
        Instantiate(bloodParticle, pos, Quaternion.identity);
    }
    internal void TakeHit(int damage, Vector3 toMoveDirection)
    {
        hp -= damage;
        animator.Play(Random.Range(0, 2) == 0 ? "TakeHit1" : "TakeHit2", 0, 0);
        // 피격 이펙트 생성
        CreateBloodEffect();
        // 뒤로 밀려나게 하자.
        PushBackMove(toMoveDirection);
        // 이동 스피드를 잠시 0으로 만들자.
        agent.speed = 0;
        CancelInvoke(nameof(SetTakeHitSpeed));
        Invoke(nameof(SetTakeHitSpeed), TakeHitStopSpeedTime);
        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(Die), 1f);
        }
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
    private void SetTakeHitSpeed()
    {
        agent.speed = originalSpeed;
    }

    void Die()
    {
        animator.Play("Die");
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    agent.destination = target.position;
    //}
}
