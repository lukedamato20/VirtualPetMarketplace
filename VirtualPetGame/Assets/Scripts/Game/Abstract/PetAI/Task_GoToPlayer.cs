using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Task_GoToPlayer : Node
{
    private Transform _transform;
    public Animator _animator;

    private Vector3 prevPosition, currentPosition, petVelocity, newPetVelocity;

    public Task_GoToPlayer(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("Player");

        if (Vector2.Distance(_transform.position, target.position) > 0.01f)
        {
            prevPosition = _transform.position;

            _transform.position = Vector2.MoveTowards(_transform.position, target.position, PetTree.speed * Time.deltaTime);

            currentPosition = _transform.position;
            petVelocity = (currentPosition - prevPosition) / Time.deltaTime;
            _animator.SetBool("isWalking", true);

            newPetVelocity = petVelocity.normalized;

            _animator.SetFloat("MoveX", newPetVelocity.x);
            _animator.SetFloat("MoveY", newPetVelocity.y);
        }

        state = NodeState.RUNNING;
        return state;
    }

}