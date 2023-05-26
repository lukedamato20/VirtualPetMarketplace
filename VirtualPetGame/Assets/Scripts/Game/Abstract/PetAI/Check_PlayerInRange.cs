using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Check_PlayerInRange : Node
{
    private Transform _transform;
    private Animator _animator;
    private static int _playerLayerMask = 1 << 6;

    public Check_PlayerInRange(Transform transform)
    {
        _transform = transform;

        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("Player");
        if (t == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, PetTree.fovRange, _playerLayerMask);

            if (colliders.Length > 0)
            {
                Debug.Log("Found! walking to player...");
                parent.parent.SetData("Player", colliders[0].transform);
                _animator.SetBool("isWalking", true);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }

}