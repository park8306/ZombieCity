using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bulletLight = GetComponentInChildren<Light>(true).gameObject;
    }
    public float speed = 5;
    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime == 0)    // 게임을 멈추고 테스트 하기 위해서
        {
            return;
        }
        Move();
        Fire();
        LookAtMouse();
        Roll();
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RollCo());
        }
    }

    private IEnumerator RollCo()
    {
        // 구르는 애니메이션 재생
        animator.SetTrigger("Roll");
        yield return null;
        // 구르는 동안 이동 스피드를 빠르게 하자.
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
            transform.Translate(move * speed * Time.deltaTime, Space.World);
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
}
