using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BehaviorTree;

public class Task_Wander : Node
{
    private Transform _transform;
    public Animator _animator;
    private Transform[] _waypoints;

    private int _currentWaypointIndex = 0, randomIndex = 0;
    private Vector3 prevPosition, currentPosition, petVelocity, newPetVelocity;
    private float _waitTime = 0f, x = 0f, y = 0f, _waitCounter = 0f;
    private bool _waiting = false;
    
    public Task_Wander(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (!PetManager.instance.interactUI.activeSelf)
        {
            if (_waiting)
            {
                _waitTime = Random.Range(1, 5);

                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                    _animator.SetBool("isWalking", true);
                }
            }
            else
            {
                if (_currentWaypointIndex == 0)
                {
                    int nextWaypointIndex = Random.Range(0, _waypoints.Length);
                    _currentWaypointIndex = nextWaypointIndex;
                }

                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    // choose the next waypoint randomly, ensuring that it's not the same as the previous waypoint
                    int nextWaypointIndex = _currentWaypointIndex;
                    while (nextWaypointIndex == _currentWaypointIndex)
                    {
                        nextWaypointIndex = Random.Range(0, _waypoints.Length);
                    }
                    _currentWaypointIndex = nextWaypointIndex;

                    _animator.SetBool("isWalking", false);
                }
                else
                {
                    prevPosition = _transform.position;
                    _transform.position = Vector3.MoveTowards(_transform.position, wp.position, PetTree.speed * Time.deltaTime);

                    currentPosition = _transform.position;
                    petVelocity = (currentPosition - prevPosition) / Time.deltaTime;
                    _animator.SetBool("isWalking", true);

                    newPetVelocity = petVelocity.normalized;

                    _animator.SetFloat("MoveX", newPetVelocity.x);
                    _animator.SetFloat("MoveY", newPetVelocity.y);
                }
            }
        }
        
        state = NodeState.RUNNING;
        return state;
    }

}