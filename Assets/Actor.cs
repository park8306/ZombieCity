using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int hp = 100;
    public GameObject bloodParticle;

    public float bloodEffectYPosition = 1.3f;
    protected Animator animator;
    protected void CreateBloodEffect()
    {
        var pos = transform.position;
        pos.y = bloodEffectYPosition;
        Instantiate(bloodParticle, pos, Quaternion.identity);
    }
}
