using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Ground and Movement Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashValue;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundChecky = 0.2f;
    [SerializeField] private float groundCheckx = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterAnimatorController characterAnimatorController;
    private float xAxis;
    private float yAxis;
    private bool canMove = true;
    private bool canCrouch = true;


    private void Awake()
    {
        characterAnimatorController = GetComponent<CharacterAnimatorController>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xAxis * walkSpeed, rb.linearVelocityY);
        characterAnimatorController.SetAnimationState("Walking", rb.linearVelocityX != 0 && isGrounded());
        characterAnimatorController.SetAnimationState("Jumping", !isGrounded());
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
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (characterAnimatorController.ReturnCurrentAnimation().Equals("Attacking1"))
            {
                characterAnimatorController.SetAnimationState("Attacking2", true);
            }
            else
            {
                characterAnimatorController.SetAnimationState("Attacking1", true);
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
            float dashDirectionValue = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 dashDirection = new Vector2(dashDirectionValue * dashValue, 0);
            if (xAxis !=0)
            {
                dashDirection *= 2;
            }

            rb.MovePosition(rb.position + dashDirection);
        }
    }

    #endregion

    #region ANIMATION_EVENTS

    public void EndDashEvent()
    {
        characterAnimatorController.SetAnimationState("Dashing", false);
    }

    public void FirstAttackEvent()
    {
        if (characterAnimatorController.ReturnCurrentAnimation().Equals("Attacking2"))
        {

        }
        else
        {
            characterAnimatorController.SetAnimationState("Attacking", false);
        }
    }

    public void SecondAttackEvent()
    {

    }

    #endregion

    public enum AnimationState
    {
        Idle,
        Walking,
        Crouching,
        Attacking1,
        Attacking2,
        Jumping,
        Dashing
    }

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
