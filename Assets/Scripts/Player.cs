using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public float speed = 5;
    public GameObject bullet;
    public Transform bulletPosition;
    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z = 1;
        if (Input.GetKey(KeyCode.S)) move.z = -1;
        if (Input.GetKey(KeyCode.D)) move.x = 1;
        if (Input.GetKey(KeyCode.A)) move.x = -1;
        if (move != Vector3.zero)
        {
            move.Normalize();
            transform.Translate(move * speed * Time.deltaTime, Space.World);
            transform.forward = move;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.Play("Shoot");
            Instantiate(bullet, bulletPosition.position, transform.rotation);
        }
        animator.SetFloat("DirX", move.x);
        animator.SetFloat("DirY", move.z);
        animator.SetFloat("Speed", move.sqrMagnitude);
    }
}
