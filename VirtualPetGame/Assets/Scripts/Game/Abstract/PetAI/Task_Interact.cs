using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Task_Interact : Node
{
    private Animator _animator;

    private PetManager _petManager;

    private Transform _lastTarget;

    public Task_Interact(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("Player");
        if (target != _lastTarget)
        {
            _petManager = target.GetComponent<PetManager>();
            _lastTarget = target;
        }

        if (PetManager.instance.readyToInteract())
        {
            Debug.Log("Starting to Interact");

            ClearData("Player");

            //Debug.Log("continuing to walk");
            _animator.SetBool("Walking", true);
        }
        else
        {
            ClearData("Player");
            Debug.Log("Pet not ready to breed...");
        }

        state = NodeState.RUNNING;
        return state;
    }

}