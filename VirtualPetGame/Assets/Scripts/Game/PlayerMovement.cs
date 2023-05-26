using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public Animator PlayerAnimator;

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x = -1;
            PlayerAnimator.SetFloat("PlayerMoveX", dir.x);
            PlayerAnimator.SetFloat("PlayerMoveY", dir.y);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x = 1;
            PlayerAnimator.SetFloat("PlayerMoveX", dir.x);
            PlayerAnimator.SetFloat("PlayerMoveY", dir.y);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.y = 1;
            PlayerAnimator.SetFloat("PlayerMoveX", dir.x);
            PlayerAnimator.SetFloat("PlayerMoveY", dir.y);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.y = -1;
            PlayerAnimator.SetFloat("PlayerMoveX", dir.x);
            PlayerAnimator.SetFloat("PlayerMoveY", dir.y);
        }

        dir.Normalize();
        PlayerAnimator.SetBool("PlayerIsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;
    }
}