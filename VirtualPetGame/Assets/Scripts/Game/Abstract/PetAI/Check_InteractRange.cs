using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Check_InteractRange : Node
{
    private Transform _transform;
    private Animator _animator;

    public Check_InteractRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("Player");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if (Vector2.Distance(_transform.position, target.position) <= PetTree.interactRange)
        {
            Debug.Log("Interacting");
            _animator.SetBool("isWalking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }

}