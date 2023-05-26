using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Check_Needs : Node
{
    private Transform _transform;
    private Animator _animator;
    private static int _playerLayerMask = 1 << 6;

    public Check_Needs(Transform transform)
    {
        _transform = transform;

        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {


        state = NodeState.SUCCESS;
        return state;
    }

}