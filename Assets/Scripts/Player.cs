using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Actor
{
    public static Player instance;
    public enum StateType
    {
        Idle,
        Move,
        TakeHit,
        Roll,
        Die,
    }
    public bool isFiring = false;
    new public Rigidbody rigidbody;
    private void Awake()
    {
        Debug.Log(Screen.safeArea.width);
        rigidbody = GetComponent<Rigidbody>();
        instance = this;
        animator = GetComponentInChildren<Animator>();
        bulletLight = GetComponentInChildren<Light>(true).gameObject;
    }
    public float speed = 5;
    public float speedWhileShooting = 3;
    //private void FixedUpdate()
    //{
    //    rigidbody.velocity = Vector3.zero;
    //}
    void Update()
    {
        if (Time.deltaTime == 0)    // 게임을 멈추고 테스트 하기 위해서
        {
            return;
        }
        if (stateType == StateType.Die)
        {
            return;
        }
        if (stateType != StateType.Roll)
        {
            Move();
            Fire();
            LookAtMouse();
            Roll(); 
        }
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RollCo());
        }
    }
    public AnimationCurve rollingSpeedAC;
    public float rollingSpeedUserMultiply = 1;
    public StateType stateType = StateType.Idle;
    private IEnumerator RollCo()
    {
        EndFiring();
        stateType = StateType.Roll;
        // 구르는 애니메이션 재생
        animator.SetTrigger("Roll");
        // 구르는 동안 이동 스피드를 빠르게 하자.
        float startTime = Time.time;    // 구르는 모션 시작하는 시간
        float endTime = startTime + rollingSpeedAC[rollingSpeedAC.length - 1].time; // 애니메이션 커브가 끝나야되는 시간
        while (endTime > Time.time) // 현재 시간이 애니메이션 커브 시간보다 작을 때 까지 실행
        {
            float time = Time.time - startTime; // 애니메이션 커브의 값을 받아오기 위한 시간 값
            float rollingSpeedMultiply = rollingSpeedAC.Evaluate(time) * rollingSpeedUserMultiply;    // 애니메이션 커브의 값을 받아온다.

            transform.Translate(transform.forward * speed * rollingSpeedMultiply * Time.deltaTime, Space.World);
            yield return null;
        }
        stateType = StateType.Idle;
        // 회전 방향은 처음 바라보던 방향으로 고정.
        // 총알 금지, 움직이는거 금지. 마우스 바라보는거 금지.
    }

    private void Move()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z = 1;
        if (Input.GetKey(KeyCode.S)) move.z = -1;
        if (Input.GetKey(KeyCode.D)) move.x = 1;
        if (Input.GetKey(KeyCode.A)) move.x = -1;
        if (move != Vector3.zero)
        {
            // 카메라 기준으로 이동
            Vector3 relateMove;
            relateMove = Camera.main.transform.forward * move.z;
            relateMove += Camera.main.transform.right * move.x;
            relateMove.y = 0;
            move = relateMove;
            move.Normalize();
            float _speed = isFiring ? speedWhileShooting : speed;
            transform.Translate(move * _speed * Time.deltaTime, Space.World);
        }
        animator.SetFloat("DirX", move.x);
        animator.SetFloat("DirY", move.z);
        animator.SetFloat("Speed", move.sqrMagnitude);
    }
    Plane plane = new Plane(new Vector3(0, 1, 0), 0);
    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray,out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - transform.position;
            dir.y = transform.position.y;
            dir.Normalize();
            transform.forward = dir;
        }
    }

    new internal void TakeHit(int damage)
    {
        base.TakeHit(damage);
        if (hp <= 0)
        {
            StartCoroutine(DieCo());
        }
        //animator
        // 
        //CreateBloodEffect();
    }
    public float diePreDelayTime=0.3f;
    private IEnumerator DieCo()
    {
        stateType = StateType.Die;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(diePreDelayTime);
        animator.SetTrigger("Die");
    }
}
