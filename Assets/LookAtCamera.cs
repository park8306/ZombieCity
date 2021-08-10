using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


[RequireComponent(typeof(LookAtConstraint))]
public class LookAtCamera : MonoBehaviour
{
    private void Awake()
    {
        var lookAtConst = GetComponent<LookAtConstraint>();

        var source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform;
        source.weight = 1;
        lookAtConst.AddSource(source);
        lookAtConst.constraintActive = true;
    }
}
