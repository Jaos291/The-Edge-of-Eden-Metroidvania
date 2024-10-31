using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Ground & Movement Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashValue = 20f;
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundChecky = 0.2f;
    [SerializeField] private float groundCheckx = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Animations & Effects")]
    [SerializeField] private CharacterAnimatorController characterAnimatorController;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector2 currentVelocity;
    private float currentXSpeed;
    private float xAxis;
    private float yAxis;
    private bool canMove = true;
    private bool canCrouch = true;
    private bool canDash = true;
    private bool performingAction = false;

    private void FixedUpdate()
    {

        characterAnimatorController.SetAnimationState("Walking", rb.linearVelocityX != 0 && isGrounded());
        characterAnimatorController.SetAnimationState("Jumping", !isGrounded());

        if (!performingAction)
        {
            rb.linearVelocity = new Vector2(xAxis * walkSpeed, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(currentVelocity.x, rb.linearVelocityY);
        }
        

    }

    #region PLAYERCONTROLLER

    public void Move(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            xAxis = context.ReadValue<Vector2>().x;
        }
        else
        {
            xAxis = 0;
        }
        if (canCrouch)
        {
            yAxis = context.ReadValue<Vector2>().y;
            Crouch();
        }

        Flip();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            canMove = true;

            if (context.performed && isGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            }

        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (characterAnimatorController.animator.GetBool("Attacking1"))
            {
                characterAnimatorController.SetAnimationState("Attacking1", false);
                characterAnimatorController.SetAnimationState("Attacking2", true);
            }
            else
            {
                characterAnimatorController.SetAnimationState("Attacking1", true);
                characterAnimatorController.SetAnimationState("Attacking2", false);
            }
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround))
        {
            canCrouch = true;
            return true;
        }
        else
        {
            canCrouch = false;
            return false;
        }

    }

    private void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    private void Crouch()
    {
        if (yAxis < 0 && isGrounded())
        {
            characterAnimatorController.SetAnimationState("Crouching", true);
            xAxis = 0;
        }
        else
        {
            characterAnimatorController.SetAnimationState("Crouching", false);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            characterAnimatorController.SetAnimationState("Dashing", true);
            StartCoroutine(GenerateDash());
        }
    }

    public IEnumerator GenerateDash()
    {
        performingAction = true;
        canMove = false;
        canDash = false;
        currentVelocity = new Vector2(dashValue * transform.localScale.x, rb.linearVelocityY);
        trailRenderer.emitting = true;
        //rb.linearVelocityX = dashValue * transform.localScale.x;
        yield return new WaitForSeconds(dashDuration);
        performingAction = false;
        canMove = true;
        canDash = true;
        trailRenderer.emitting = false;
    }

    #endregion

    #region ANIMATION_EVENTS

    public void EndDashEvent()
    {
        characterAnimatorController.SetAnimationState("Dashing", false);
    }

    public void AttackCycleEvent(string attackCycle)
    {
        characterAnimatorController.SetAnimationState(attackCycle, false);
    }

    #endregion

    //OLD METHODS
    /*private void Update()
    {
        GetInputs();
        Move();
        Jump();
    }

    private void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        }

        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocityY);
    }

    private bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckx,0,0), Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }*/
}
