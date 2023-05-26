using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInteractionMovement : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float xLimitLeft;
    public float xLimitRight;
    public float yLimitTop;
    public float yLimitBottom;

    private bool isWaiting = false;
    private float waitTime = 0f, waitCounter = 0f;
    private bool isMoving = false;

    private Vector2 targetPosition, prevPosition, currentPosition, petVelocity, newPetVelocity;

    void Start()
    {
        SetNewTarget();
        animator.SetBool("isWalking", false);
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTime = Random.Range(1, 5);
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                isMoving = true;
                animator.SetBool("isWalking", true);
            }
        }
        else if (isMoving)
        {
            prevPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            currentPosition = transform.position;
            petVelocity = (currentPosition - prevPosition) / Time.deltaTime;
            animator.SetBool("isWalking", true);

            newPetVelocity = petVelocity.normalized;
            animator.SetFloat("MoveX", newPetVelocity.x);
            animator.SetFloat("MoveY", newPetVelocity.y);

            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                animator.SetBool("isWalking", false);
                SetNewTarget();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void SetNewTarget()
    {
        float x = Random.Range(xLimitLeft, xLimitRight);
        float y = Random.Range(yLimitBottom, yLimitTop);

        // Clamp the x and y values within the limits
        x = Mathf.Clamp(x, xLimitLeft, xLimitRight);
        y = Mathf.Clamp(y, yLimitBottom, yLimitTop);

        targetPosition = new Vector2(x, y);
        isWaiting = true;
        waitCounter = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
        animator.SetBool("isWalking", false);
    }
}