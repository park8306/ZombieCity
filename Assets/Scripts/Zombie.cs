using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    public int hp = 100;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        while (hp > 0)
        {
            if (target)
            {
                agent.destination = target.position;
            }
            yield return new WaitForSeconds(Random.Range(0, 0.5f));
        }
    }

    internal void TakeHit(int damage)
    {
        hp -= damage;
        animator.Play("TakeHit");
        // 피격 이펙트 생성
        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(Die), 1f);
        }
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
