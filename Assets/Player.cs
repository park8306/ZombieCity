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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.Play("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.Play("Run");
        }
    }
}
