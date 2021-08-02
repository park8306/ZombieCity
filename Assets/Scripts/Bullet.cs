using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float destroyTime = 1f;
    public int power = 20;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie")) // 일반적으로 tag로 비교하면 GC가 발생한다.
        {
            var zombie = other.GetComponent<Zombie>();
            zombie.TakeHit(power);
            Destroy(gameObject);
        }
    }
}
